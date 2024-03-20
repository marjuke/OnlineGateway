using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using SOAP.Authorization;
using SOAP.Model;
namespace SOAP.Authentication.DataStores.File;
public class FileAuthDataStore : IAuthenticationRepository, IAuthorizationRepository
{
    private readonly ILogger _logger;
    protected const string DefaultAuthDataStoreFolderName = "SOAP/Authentication/SOAPAuthentication/DataStore/File";
    protected const string DefaultAuthDataStoreFileName = "AuthDataStore.json";
    protected const string AuthDataStoreFileNameConfigKey = "SOAPAuthenticationDataStore:FilePath";
    protected IDictionary<string, AuthEntry> users = new Dictionary<string, AuthEntry>();
    protected IPasswordEncoder _passwordEncoder;
    public FileAuthDataStore(IConfiguration configuration, ILogger<FileAuthDataStore> logger, IPasswordEncoder passwordEncoder)
    {
        _logger = logger;
        _passwordEncoder = passwordEncoder;
        string? filePath = configuration[AuthDataStoreFileNameConfigKey];
        if (filePath is null)
        {
            string? baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (baseDir is null)
                throw new FileNotFoundException("FileAuthDataStore cannot find the Auth Information File Directory");
            string[] files = Directory.GetFiles(baseDir, DefaultAuthDataStoreFileName, SearchOption.TopDirectoryOnly);
            if (files.Count() != 1)
            {
                files = Directory.GetFiles(baseDir, DefaultAuthDataStoreFileName, SearchOption.AllDirectories);
                if (files.Count() == 0)
                    throw new FileNotFoundException("FileAuthDataStore cannot find the Auth Information File");
                if (files.Count() > 1)
                    throw new FileNotFoundException("FileAuthDataStore found more than one Auth Information File, there can be only one!");
            }
            filePath = files[0];
        }
        string jsonString = System.IO.File.ReadAllText(filePath);
        using (JsonDocument document = JsonDocument.Parse(jsonString))
        {
            foreach (JsonElement userElement in document.RootElement.EnumerateArray())
            {
                string userid = userElement.GetMandatoryString("UserID");
                string username = userElement.GetMandatoryString("Username");
                string password = userElement.GetMandatoryString("Password");

                List<String>? roles = null;
                JsonElement rolesColl;
                if (userElement.TryGetProperty("Roles", out rolesColl))
                {
                    roles = new List<string>();
                    foreach (JsonElement roleElement in rolesColl.EnumerateArray())
                    {
                        string? role = roleElement.GetString();
                        if (role is not null)
                            roles.Add(role);
                    }
                }

                List<PermissionItem>? allowedPermissions = ReadPermissionCollection(userElement, "Allow");
                List<PermissionItem>? deniedPermissions = ReadPermissionCollection(userElement, "Deny");

                AuthEntry user = new AuthEntry(userid, username, password, roles, allowedPermissions, deniedPermissions);
                users.Add(new KeyValuePair<string, AuthEntry>(user.UserID, user));
            }
        }
        if (users.Count == 0)
            _logger.LogWarning("No users added to Auth DataStore. This might break the Authentication mechanism.");
    }
    protected List<PermissionItem>? ReadPermissionCollection(JsonElement parent, string permissionPropertyName)
    {
        List<PermissionItem>? permissions = null;
        JsonElement permissionsColl;
        if (parent.TryGetProperty(permissionPropertyName, out permissionsColl))
        {
            permissions = new List<PermissionItem>();
            foreach (JsonElement serviceElement in permissionsColl.EnumerateArray())
            {
                var enumerator = serviceElement.EnumerateObject();
                if (enumerator.Count() > 0)
                {
                    JsonProperty serviceObject = enumerator.First();
                    List<String>? allowedOperations = ReadOperationCollection(serviceObject.Value, "Allow");
                    List<String>? deniedOperations = ReadOperationCollection(serviceObject.Value, "Deny");
                    string service = serviceObject.Name;
                    permissions.Add(new PermissionItem(service, allowedOperations, deniedOperations));
                }
            }
        }
        return permissions;
    }
    protected List<String>? ReadOperationCollection(JsonElement parent, string permissionPropertyName)
    {
        List<String>? operations = null;
        JsonElement operationsColl;
        if (parent.TryGetProperty(permissionPropertyName, out operationsColl))
        {
            operations = new List<String>();
            foreach (JsonElement operationElement in operationsColl.EnumerateArray())
            {
                string? operation = operationElement.GetString();
                if (operation is not null)
                    operations.Add(operation);
            }
        }
        return operations;
    }


    public virtual Task<bool> AuthenticateUserAsync(ClaimsPrincipal principal, Object? authDataObject)
    {
        bool bAuthenticated = false;
        Claim? userIdClaim = principal.Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
        if (userIdClaim != null)
        {
            if (users.ContainsKey(userIdClaim.Value))
            {
                AuthEntry userAuth = users[userIdClaim.Value];
                if (authDataObject is null || authDataObject.GetType() != typeof(String))
                    return Task.FromResult(false);
                string password = (string)authDataObject;
                bool isAuthenticated = _passwordEncoder.matches(password, userAuth.Password).matches;
                if (isAuthenticated && typeof(ClaimsIdentity).IsAssignableFrom(principal.Identity?.GetType())) // Augment the Principal
                {
                    var identity = (ClaimsIdentity)principal.Identity;
                    identity.AddClaim(new Claim(identity.NameClaimType, userAuth.Username));
                    if (userAuth.Roles is not null)
                        foreach (string role in userAuth.Roles)
                            identity.AddClaim(new Claim(identity.RoleClaimType, role));
                }
                return Task.FromResult(isAuthenticated);
            }
        }
        else
            throw new FileAuthDataStoreException($"User does not have the claim {ClaimTypes.NameIdentifier}. This is required by this Auth DataStore. The Authentication plugin should add this claim for the user");
        return Task.FromResult(bAuthenticated);
    }

    public Task<bool> AuthorizeUserAsync(ClaimsPrincipal principal, object? authDataObject)
    {
        if (authDataObject is null || !typeof(SOAPAuthData).IsAssignableFrom(authDataObject.GetType()))
            throw new FileAuthDataStoreException("FileAuthDataStore expects a SOAPAuthData object. The AuthorizeAttribute should supply this.");
        SOAPAuthData authZData = (SOAPAuthData)authDataObject;

        if (authZData.RequestedDepartments is not null)
        {
            var depts = authZData.RequestedDepartments.Split(",").Select(x => x.Trim());
            //Validate the user in in these departments
        }

        if (authZData.PerformSOAPOperationAuthorization)
        {
            //Authorization could be called multiple times i.e. if there are multiple attributes on the controller
            //This flag ensures we only perform SOAP Operation authorization once
            authZData.PerformSOAPOperationAuthorization = false;
            // userIdClaim must be present here, otherwise Authentication would have failed
            Claim userIdClaim = principal.Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).First();
            AuthEntry userAuth = users[userIdClaim.Value];
            bool bAuthorized = false;
            if (userAuth.AllowedPermissions is not null)
            {
                // This is a whitelist so deny everything except the entries in the collection
                bAuthorized = false;
                var permissionItem = userAuth.AllowedPermissions.FirstOrDefault(x => x.Service == authZData.TargetServiceName);
                if (permissionItem is not null)
                {
                    bAuthorized = true;
                    if (permissionItem.AllowedOperations is not null && !permissionItem.AllowedOperations.Contains(authZData.RequestedSOAPOperationName))
                        bAuthorized = false;
                    if (permissionItem.DeniedOperations is not null && permissionItem.DeniedOperations.Contains(authZData.RequestedSOAPOperationName))
                        bAuthorized = false;
                }
            }
            else
            {
                if (userAuth.DeniedPermissions is not null)
                {
                    // This is a Blacklist so allow everything except the entries in the collection
                    bAuthorized = true;
                    var permissionItem = userAuth.DeniedPermissions.FirstOrDefault(x => x.Service == authZData.TargetServiceName);
                    if (permissionItem is not null)
                    {
                        bAuthorized = false;
                        if (permissionItem.AllowedOperations is not null && permissionItem.AllowedOperations.Contains(authZData.RequestedSOAPOperationName))
                            bAuthorized = true;
                        if (permissionItem.DeniedOperations is not null && !permissionItem.DeniedOperations.Contains(authZData.RequestedSOAPOperationName))
                            bAuthorized = true;
                    }
                }
                else
                {
                    // Neither Allow or Deny has been specified, so take some default action (Deny access)
                    bAuthorized = false;
                }
            }
            return Task.FromResult(bAuthorized);
        }
        return Task.FromResult(true);
    }
}

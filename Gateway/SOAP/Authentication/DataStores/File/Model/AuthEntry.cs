namespace SOAP.Authentication.DataStores.File;
public class AuthEntry
{
    public AuthEntry(string userId, string username, string password, IEnumerable<String>? roles, IEnumerable<PermissionItem>? allowedPermissions, IEnumerable<PermissionItem>? deniedPermissions)
    {
        UserID = userId;
        Username = username;
        Password = password;
        Roles = roles;
        AllowedPermissions = allowedPermissions;
        DeniedPermissions = deniedPermissions;
    }
    public string UserID { get; }
    public string Username { get; }
    public string Password { get; }
    public IEnumerable<String>? Roles { get; }
    public IEnumerable<PermissionItem>? AllowedPermissions { get; }
    public IEnumerable<PermissionItem>? DeniedPermissions { get; }

}
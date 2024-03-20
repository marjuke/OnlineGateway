namespace SOAP.Authentication.DataStores.File;
public class PermissionItem
{
    public PermissionItem(string service, IEnumerable<String>? allowedOperations, IEnumerable<String>? deniedOperations)
    {
        if (allowedOperations is not null && deniedOperations is not null)
            throw new ArgumentException("PermissionItem cannot have both Allow and Deny collections.");
        Service = service;
        AllowedOperations = allowedOperations;
        DeniedOperations = deniedOperations;
    }
    public string Service { get; set; }
    public IEnumerable<String>? AllowedOperations { get; }
    public IEnumerable<String>? DeniedOperations { get; }
}
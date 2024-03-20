using System.Security.Claims;
namespace SOAP.Authorization;
public interface IAuthorizationRepository
{
    Task<bool> AuthorizeUserAsync(ClaimsPrincipal principal, object? authData);
}
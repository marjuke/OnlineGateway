using System.Security.Claims;
namespace SOAP.Authentication;
public interface IAuthenticationRepository
{
    Task<bool> AuthenticateUserAsync(ClaimsPrincipal principal, object? authData);
}

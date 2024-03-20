using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SOAP.Authentication;
using SOAP.Authentication.Handlers.SOAP;
using SOAP.Utilities;
namespace SOAP.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class SOAPAuthenticationUtilitiesController : ControllerBase
{
    private readonly ILogger<SOAPAuthenticationUtilitiesController> _logger;
    private readonly IPasswordEncoder? _passwordEncoder;
    private readonly IAuthenticationRepository? _authenticationRepository;
    // We accept a collection of password encoders in case authentication is not Added i.e. not required then there will not be any IPasswordEncoder registered with DI.
    public SOAPAuthenticationUtilitiesController(ILogger<SOAPAuthenticationUtilitiesController> logger, IEnumerable<IPasswordEncoder> passwordEncoders, IEnumerable<IAuthenticationRepository> authenticationRepositories)
    {
        _logger = logger;
        _passwordEncoder = passwordEncoders.FirstOrDefault();
        _authenticationRepository = authenticationRepositories.FirstOrDefault();
    }
    public IActionResult GetEncodedPassword(string? password)
    {
        if (_passwordEncoder is null)
            return Problem("Password Encoder is not configured.", statusCode: StatusCodes.Status501NotImplemented);
        if (password is null)
        {
            RandomPasswordGenerator gen = new RandomPasswordGenerator();
            password = gen.GeneratePassword(true, true, true, false, 16);
        }
        var encodedpassword = _passwordEncoder.encode(password);
        return Ok(new { password = password, encodedPassword = encodedpassword });
    }
    public async Task<IActionResult> TestAuthentication(string userid, string password)
    {
        if (_authenticationRepository is null)
            return Problem("Authentication Repository is not configured.", statusCode: StatusCodes.Status501NotImplemented);
        ClaimsPrincipal principal = new ClaimsPrincipal();
        ClaimsIdentity identity = new ClaimsIdentity(SOAPAuthenticationDefaults.AuthenticationScheme + "Identity");
        principal.AddIdentity(identity);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userid));
        return Ok(new { IsValid = await _authenticationRepository.AuthenticateUserAsync(principal, password) });
    }
}

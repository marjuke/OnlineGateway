using Domain.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gateway.Services
{
    public class TokenService
    {
        //private readonly string _key;
        //public TokenService(string key)
        //{
        //    _key = key;
        //}
        public string GenerateToken(AppUser appUser)
        {
            var clamis = new List<Claim>
            {
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.NameIdentifier, appUser.UserID),
                new Claim(ClaimTypes.Email, appUser.Email)
            };
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4Zs6r0AEnoLgN0GaRirrdL8RLUvwume4xQGBLjl3DKBTYwYnum1etLV2kjE7"));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4Zs6r0AEnoLgN0GaRirrdL8RLUvwume4xQGBLjl3DKBTYwYnum1etLV2kjE7dsfdsfdfdsfdsfewrfdfDFSDFASDDdsfdsfewr23432adadsdsadfdsfdsfdsfdsfdsfadsfdsfdsafafafasdfsfewrewrdsfadsfadsfads"));
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(clamis),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        //public string ValidateToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_key);
        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);
        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        var UserID = jwtToken.Claims.First(x => x.Type == "UserID").Value;
        //        return UserID;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
    }
}

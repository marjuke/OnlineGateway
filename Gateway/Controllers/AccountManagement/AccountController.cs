using Domain.DTOs;
using Domain.Identity;
using Gateway.Model.Password;
using Gateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers.MemberAppCon
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //account registration login and logout with jwt token
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        //public AccountController(UserManager<AppUser> userManager)
        //{
        //    _userManager = userManager;
        //    //_tokenService = tokenService;
        //}
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            //var password = "432432432";
            //var key = "apTN9ruR3hef1d1cQ4zylWRSeXmXwwKZg98NiKdrEmI=";
            //var password1 = PasswordDecryptor.GetKeyWithOutSubStringEncrypt(password, key);
            //var password2 = PasswordDecryptor.GetPasswordWithOutSubString(password1, key);
            //var password = "Admin@123";
            //string IV = "bgdNbifdTbidMK+D/aSePQ==";
            //var key = "uY8PZNda3xRrot1JmnYcsL4/yyuf+uqCF7KvYByD12c=";
            
            //string IV = "Cv1CJmrifzOQsSfldGV4Sw==";
            //var key = "XC7ur+RqMGVpzAZuxgrfT2NedilRYxrBX5Nw1eEeB0g=";
            var enpassword = loginDto.Password;
            var d1 = enpassword.Take(44).ToArray();
            var s2 = enpassword.TakeLast(24).ToArray();


            string d = String.Join("", d1.Select(p => p.ToString()).ToArray());
            string s = String.Join("", s2.Select(p => p.ToString()).ToArray());


            //var modifiedresult = RemoveFirstAndLast(result, 6, 12);
            string modifiedString = enpassword.Substring(44, enpassword.Length - 44 - 24);

            //var password1 = EncryptionHelper.Encrypt(password, key, IV);
            //var password2 = EncryptionHelper.Decrypt(modifiedString, key,IV);
            var password = EncryptionHelper.Decrypt(modifiedString, d, s);

            //var user = await _userManager.FindByIdAsync(loginDto.UserID);
            var user = _userManager.Users.Where(s => s.UserID == loginDto.UserID).FirstOrDefault();
            if (user == null) return Unauthorized("Invalid User");
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result) return Unauthorized("Invalid Password");
            return new UserDTO
            {
                UserID = user.UserID,
                Token = _tokenService.GenerateToken(user),
                //Token = "This is token",
                //DisplayName = user.DisplayName
            };
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserID == registerDto.UserID))
            {
                return BadRequest("User is taken");
            }
            var user = new AppUser
            {
                UserID = registerDto.UserID,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                BranchCode = registerDto.BranchCode,
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                EmailConfirmed = false,
                ChannelListID = registerDto.ChannelID,


                //DisplayName = registerDto.DiaplayName
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest("Problem registering user");
            return new UserDTO
            {
                UserID = user.UserID,
                //Token = _tokenService.GenerateToken(user.UserID),
                Token = "This is token",
                //DisplayName = user.DisplayName
            };
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}

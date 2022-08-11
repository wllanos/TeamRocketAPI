using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamRocketAPI.DTOs;

namespace TeamRocketAPI.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;


        //service inyection to register an user
        public AccountsController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Register API user
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        [HttpPost("register")]//api/accounts/register
        public async Task<ActionResult<AuthenticationResponse>> Register
            (UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email,
                Email = userCredentials.Email };
            var result = await userManager.CreateAsync(user, userCredentials.Password);

            if(result.Succeeded)
            {
                return TokenBuilder(userCredentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Login API user
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns>User token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentials userCredentials)
        {
            var result = await signInManager.PasswordSignInAsync(userCredentials.Email,
                userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return TokenBuilder(userCredentials);
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }

        /// <summary>
        /// Every time a user sends a token, claims can be readed 
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        private AuthenticationResponse TokenBuilder(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email)
            };
            //signing the jwt
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, 
                claims: claims, expires: expiration, signingCredentials: credentials);

            //return a string represents token
            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };

        }
    }
}

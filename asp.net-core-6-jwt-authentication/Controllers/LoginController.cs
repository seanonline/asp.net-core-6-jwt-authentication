using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using asp.net_core_6_jwt_authentication.Models;
using asp.net_core_6_jwt_authentication.Service;

namespace asp.net_core_6_jwt_authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserInterface userService;

        public LoginController(IUserInterface _userService,IConfiguration configuration)
        {
            this.userService = _userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public ActionResult<object> Authenticate([FromBody] LoginRequest login)
        {
            var loginResponse = new LoginResponse(string.Empty,string.Empty,string.Empty) { } ;
            LoginRequest loginrequest = login;

            bool isUsernamePasswordValid = false;

            if (login != null)
            {
                // make await call to the Database to check username and password. here we only checking if password value is admin
                isUsernamePasswordValid = loginrequest.Password == "admin" ? true : false;
            }
            // if credentials are valid
            if (isUsernamePasswordValid)
            {
                string token = CreateToken(loginrequest.UserName);

                loginResponse.Token = token;
              
                loginResponse.RefreshToken = "xxxxxxtestRefreshToken";
                loginResponse.UserId = loginrequest.UserName;

                this.userService.RecordUserLogin(loginResponse);

                //return the token
                return Ok(new { loginResponse });
            }
            else
            {
                // if username/password are not valid send unauthorized status code in response               
                return BadRequest("Username or Password Invalid!");
            }
        }

        private string CreateToken(string username)
        {

            List<Claim> claims = new()
            {                    
                //list of Claims - we only checking username - more claims can be added.
                new Claim("username", Convert.ToString(username)),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}

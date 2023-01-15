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
            var loginResponse = new LoginResponse(string.Empty,string.Empty,DateTime.UtcNow) { } ;
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

                loginResponse.RefreshToken = new RefreshToken(string.Empty, string.Empty, DateTime.UtcNow,string.Empty);


                loginResponse.RefreshToken = CreateRefreshToken(loginrequest.UserName);
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

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] LoginRequest refreshTokenResource)
        {
            // find username + accessToken combination in the database

            var user = this.userService.GetUser(refreshTokenResource.UserName);
            var refreshtoken = this.userService.GetRefreshTokenDetail(refreshTokenResource.UserName);

            if (user == null) { return BadRequest(); }
            if (refreshtoken == null) { return BadRequest(); }
            if (user.Token != refreshTokenResource.Token) { return BadRequest("Not a Valid Access Token"); };

            if (refreshtoken.Expiration < DateTime.Now) {

                this.userService.RemoveRefreshToken(refreshTokenResource.UserName,refreshtoken.Token);
                return BadRequest("Refresh Token Expired. Please Login again"); 
            };

            //Update the user access token + refresh token
             
            var loginResponse = new LoginResponse(string.Empty, string.Empty, DateTime.UtcNow) { };

            string token = CreateToken(refreshTokenResource.UserName);
            loginResponse.AccessTokenExpiry = DateTime.Now.AddMinutes(2);

            loginResponse.Token = token;

            loginResponse.RefreshToken = new RefreshToken(string.Empty, string.Empty, DateTime.UtcNow,string.Empty);


            loginResponse.RefreshToken = refreshtoken;
            loginResponse.UserId = refreshTokenResource.UserName;


            //update database with new access token/refresh token combination for the user.
            this.userService.RecordUserLogin(loginResponse);


            return Ok(new { loginResponse });

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
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        // Refresh Token Creation

        private RefreshToken CreateRefreshToken(string id)
        {
            var refreshToken = new RefreshToken
            (
                token: Guid.NewGuid().ToString(),
                expiration: DateTime.Now.AddMinutes(5),
                tokenType: "Refresh",
                 username:id
            );
            return refreshToken;
        }


    }
}

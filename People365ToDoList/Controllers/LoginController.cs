using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using People365ToDoList.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace People365ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost, Route("login")]
        public IActionResult Login(Login loginDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginDTO.UserName) ||
                    string.IsNullOrEmpty(loginDTO.Password))
                {
                    return BadRequest("Username and/or Password not specified");
                }

                if (loginDTO.UserName.Equals("people365") &&
                    loginDTO.Password.Equals("P@ssw0rd"))
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "Mohammad",
                        audience: "https://localhost:7001",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: signinCredentials
                    );

                    // Return a 200 OK response with the JWT token
                    return Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
                }

                // Return Unauthorized for incorrect username or password
                return Unauthorized("Wrong Username and/or Password");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred in generating the token {ex.Message}");
            }
        }
    }
}

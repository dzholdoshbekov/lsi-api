using DoctorApi.Models;
using DoctorApi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DoctorApi.Controllers
{
    [Route("auth")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _context;

        public LoginController(IConfiguration config, DatabaseContext context)
        {
            _config = config;
            _context = context;
        }
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user =  _context.Users.FirstOrDefault(user => user.Username == login.Username);

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password)) {
                    var tokenString = GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenString });
                } else {
                    response = Unauthorized(new { message = "Authorization failed. Username or password is incorrect." });
                }

                if (!user.IsAdmin)
                {
                    response = Unauthorized(new { message = "User is not admin." });
                }
                
            }

            return response;
        }

        [AllowAnonymous]
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] UserModel signup)
        {
            IActionResult response = Unauthorized();
            var user = _context.Users.FirstOrDefault(user => user.Username == signup.Username);

            var password = BCrypt.Net.BCrypt.HashPassword(signup.Password);

            if (user == null)
            {
                await _context.Users.AddAsync(
                    new UserModel
                    {
                        Username = signup.Username,
                        Password = password,
                        Bio = signup.Bio,
                        Email = signup.Email
                    });

                await _context.SaveChangesAsync();

                response = Ok(new { message = "User created successfully!" });
            }

            return response;

        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddDays(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


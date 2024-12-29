using Lesson30_WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lesson30_WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase//auth sorgularini proses edip geriye token qaytarmaq
    {
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private List<User> _users = new List<User>()
        {
            new User()
            {
                Id=1,
                FName="Samir",
                LName="Eldarov",
                UserName="samir.eldarov",
                Password="Samir2003",
                Roles=new List<string>()
                {
                    "SuperAdmin"
                }
            },
            new User()
            {
                Id=2,
                FName="Cumar",
                LName="Yusifov",
                UserName="cumar.yusifov",
                Password="Cumar1997",
                Roles=new List<string>()
                {
                    "User"
                }
            },
            new User()
            {
                Id=3,
                FName="Idris",
                LName="Abbasov",
                UserName="idris.abbasov",
                Password="Idris2004",
                Roles=new List<string>()
                {
                    "Admin",
                    "User"
                }
            }
        };
        private readonly IConfiguration _configuration;
        //private readonly object _configuratin;

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            var user = _users.FirstOrDefault(x => x.UserName == loginModel.UserName);
            if (user==null)
            {
                return NotFound(new
                {
                    message="UserName or Password is wrong"
                });
            }
            var userr = HttpContext.User;//ClaimsPrincipal,ClaimsIdentity arxada ClaimsPrincipal-e cevrilir aradaki ferq prinsipli bir sexs  identity-i senedler kimi
            
            var token = GenerateJWTToken(user);
            return Ok(new LoginResultModel()
            {
                UserId=user.Id,
                AuthToken=token
            });

        }


        private string GenerateJWTToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWTKey:SigningKey"]);
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.FName)
                };
            if (user.Roles.Count>0)
            {
                claims.AddRange(user.Roles.Select(item => new Claim(ClaimTypes.Role, item)));
                //claims.Add(new Claim(ClaimTypes.Role, string.Join(",", user.Roles)));
            }
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),               
                Issuer = "example.com",
                Audience = "example.com",
                Expires =DateTime.UtcNow.AddDays(7),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);//strnge cevirir
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}

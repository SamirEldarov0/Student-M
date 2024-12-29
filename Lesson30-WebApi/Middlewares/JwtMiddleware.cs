using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lesson30_WebApi.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JwtMiddleware(RequestDelegate next,IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                attachUserToContext(context, token);
            }
            await _next(context);
        }

        private void attachUserToContext(HttpContext context,string token)
        {
            //indi token icerisindeki melumatlari oxuyuram
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["JWTKey:SigningKey"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters//token gelende hansi seyleri validate ele
                {
                    ValidateIssuerSigningKey = true,//key-i validate ele 
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,//tokeni kim generate edip
                    ValidateAudience=false,//token kimin ucun nezerde tutulub
                    ClockSkew=TimeSpan.Zero
                },out SecurityToken validatedToken) ;
                var jwtToken=(JwtSecurityToken)validatedToken;
                var identity = new ClaimsIdentity(jwtToken.Claims);
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;
            }
            catch 
            {
                //exception bas ver se context.User-e hecne assign olmuyacaq ve authenticationdan kecmeyecek
            }
        }
    }
}

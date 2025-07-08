using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class MyJWT
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyJWT(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }


        public string GenerateJSONWebToken(User client)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim("UserName", client.UserName.ToString())
            };

            var tokeOptions = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(_config["JWT:ExpireIn"])),
                signingCredentials: signinCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return token;
        }



        public string GetUserNameFromToken()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            string token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                // Token not found in the header
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userNameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserName");
            return userNameClaim?.Value;
        }

        public int GetAuthUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity != null && user.Identity.IsAuthenticated)
            {
                var userValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userValue != null) return int.Parse(userValue);
            }
            return 0;

        }
    }

}

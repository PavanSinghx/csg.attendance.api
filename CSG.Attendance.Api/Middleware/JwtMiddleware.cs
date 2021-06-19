using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using CSG.Attendance.Api.Models;

namespace CSG.Attendance.Api.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JwtSettings jwtSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> appSettings)
        {
            this.next = next;
            this.jwtSettings = appSettings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                AddUserToHttpContext(context, token);
            }

            await this.next(context);
        }

        private void AddUserToHttpContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var jwtSecret = Encoding.ASCII.GetBytes(jwtSettings.Secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                context.Items["firebaseid"] = jwtToken.Claims.FirstOrDefault(c => c.Type == "firebaseid");
            }
            catch { }
        }
    }
}

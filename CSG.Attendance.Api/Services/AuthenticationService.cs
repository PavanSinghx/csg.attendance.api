using CSG.Attendance.Api.Exceptions;
using CSG.Attendance.Api.Extensions;
using CSG.Attendance.Api.Models;
using CSG.Attendance.Api.Models.Mappings;
using CSG.Attendance.Api.Models.Response;
using CSG.Attendance.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings jwtSettings;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRepository<TbTeacher> teacherRepository;
        private readonly IMemoryCacheService memoryCacheService;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IOptionsMonitor<JwtSettings> monitor, IRepository<TbTeacher> teacherRepository, IMemoryCacheService memoryCacheService)
        {
            this.jwtSettings = monitor.CurrentValue;
            this.httpContextAccessor = httpContextAccessor;
            this.teacherRepository = teacherRepository;
            this.memoryCacheService = memoryCacheService;
        }

        public async Task<JwtResponse> GetJwtToken(string firebaseId)
        {
            firebaseId.ThrowIfNullEmptyOrWhiteSpace("FirebaseId");

            var cachedValue = this.memoryCacheService.RetrieveValue<string, int>(firebaseId);

            if (cachedValue == default)
            {
                var teacherExists = await teacherRepository.FirstOrDefaultAsync(t => t.FirebaseUid == firebaseId);

                if (teacherExists == default)
                {
                    throw new UserNotFoundException(firebaseId);
                }
                else
                {
                    this.memoryCacheService.SetValue<string, int>(teacherExists.FirebaseUid, teacherExists.TeacherId);
                }
            }

            var expiryDate = DateTime.UtcNow.AddHours(jwtSettings.ExpiryTimeInHours);
            var token = this.GenerateJwtToken(firebaseId, expiryDate);

            var jwtSummary = new JwtResponse
            {
                Token = token,
                ExpiryDate = expiryDate.ToString()
            };

            return jwtSummary;
        }

        private string GenerateJwtToken(string firebaseUid, DateTime expiryDate)
        {
            var jwtSecret = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            var claim = new List<Claim>
            {
                new Claim(this.jwtSettings.ClaimsIdentifier, firebaseUid)
            };

            var claimsIdentity = new ClaimsIdentity(claim);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecret), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Service.Data;
using UserManagement.Service.Models;
using UserManagement.Service.Protos;

namespace UserManagement.Service.Services
{
    public class GrpcUserService : UserService.UserServiceBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public GrpcUserService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public override async Task<AuthenticateReply> Authenticate(AuthenticateRequest request, ServerCallContext context)
        {
            var user = _db.Users.SingleOrDefault(u => u.Username == request.Username);
            if (user == null) return new AuthenticateReply { Success = false, Message = "Invalid credentials" };

            var verified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!verified) return new AuthenticateReply { Success = false, Message = "Invalid credentials" };

            // create JWT
            var jwt = _config.GetSection("Jwt");
            var key = jwt.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key missing");
            var issuer = jwt.GetValue<string>("Issuer") ?? "user-micro";
            var audience = jwt.GetValue<string>("Audience") ?? "user-clients";

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("email", user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthenticateReply
            {
                Success = true,
                Message = "Authenticated",
                Token = tokenString,
                User = new UserMessage
                {
                    Id = user.Id.ToString(),
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? "",
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt.ToString("o")
                }
            };
        }

        public override async Task<GetUserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var uid))
            {
                return new GetUserReply { Found = false, Message = "Invalid user id" };
            }

            var user = _db.Users.SingleOrDefault(u => u.Id == uid);
            if (user == null) return new GetUserReply { Found = false, Message = "Not found" };

            return new GetUserReply
            {
                Found = true,
                Message = "OK",
                User = new UserMessage
                {
                    Id = user.Id.ToString(),
                    Username = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? "",
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt.ToString("o")
                }
            };
        }
    }
}
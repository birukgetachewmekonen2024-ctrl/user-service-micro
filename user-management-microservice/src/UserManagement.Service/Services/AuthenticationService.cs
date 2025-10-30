using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using UserManagement.Service.DTOs;
using UserManagement.Service.Models;
using UserManagement.Service.Repositories;
using UserManagement.Service.Security;

namespace UserManagement.Service.Services
{
    public class AuthenticationService : AuthenticationService.AuthenticationServiceBase
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtTokenProvider _jwtTokenProvider;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IUserRepository userRepository, PasswordHasher passwordHasher, JwtTokenProvider jwtTokenProvider, ILogger<AuthenticationService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenProvider = jwtTokenProvider;
            _logger = logger;
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetUserByUsername(request.Username);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid login attempt for user {Username}", request.Username);
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid username or password"));
            }

            var token = _jwtTokenProvider.GenerateToken(user);
            return new LoginResponse { Token = token };
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var existingUser = await _userRepository.GetUserByUsername(request.Username);
            if (existingUser != null)
            {
                _logger.LogWarning("User {Username} already exists", request.Username);
                throw new RpcException(new Status(StatusCode.AlreadyExists, "User already exists"));
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email
            };

            await _userRepository.AddUser(newUser);
            _logger.LogInformation("User {Username} registered successfully", request.Username);
            return new RegisterResponse { Success = true };
        }
    }
}
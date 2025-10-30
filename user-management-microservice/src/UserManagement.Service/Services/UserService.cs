using System.Threading.Tasks;
using UserManagement.Service.Models;
using UserManagement.Service.DTOs;
using UserManagement.Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserManagement.Service.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> RegisterUserAsync(UserRegistrationDto registrationDto)
        {
            var user = new User
            {
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                PasswordHash = HashPassword(registrationDto.Password) // Assume HashPassword is implemented
            };

            await _userRepository.AddUserAsync(user);
            _logger.LogInformation("User registered: {Username}", user.Username);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
            }
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
            }
            return user;
        }

        private string HashPassword(string password)
        {
            // Implement password hashing logic here
            return password; // Placeholder
        }
    }
}
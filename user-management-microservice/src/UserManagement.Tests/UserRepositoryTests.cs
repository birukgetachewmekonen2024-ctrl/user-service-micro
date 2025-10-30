using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using UserManagement.Service.Data;
using UserManagement.Service.Models;
using UserManagement.Service.Repositories.Impl;

namespace UserManagement.Tests
{
    public class UserRepositoryTests
    {
        private readonly Mock<UserDbContext> _mockContext;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _mockContext = new Mock<UserDbContext>();
            _userRepository = new UserRepository(_mockContext.Object);
        }

        [Fact]
        public async Task AddUser_ShouldAddUserToDatabase()
        {
            var user = new User { Id = 1, Username = "testuser", PasswordHash = "hashedpassword", Email = "test@example.com" };

            await _userRepository.AddUser(user);

            _mockContext.Verify(m => m.Users.AddAsync(user, default), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser", PasswordHash = "hashedpassword", Email = "test@example.com" };

            _mockContext.Setup(m => m.Users.FindAsync(userId)).ReturnsAsync(user);

            var result = await _userRepository.GetUserById(userId);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUserByUsername_ShouldReturnUser_WhenUserExists()
        {
            var username = "testuser";
            var user = new User { Id = 1, Username = username, PasswordHash = "hashedpassword", Email = "test@example.com" };

            _mockContext.Setup(m => m.Users.FirstOrDefaultAsync(u => u.Username == username)).ReturnsAsync(user);

            var result = await _userRepository.GetUserByUsername(username);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetUserByUsername_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var username = "nonexistentuser";

            _mockContext.Setup(m => m.Users.FirstOrDefaultAsync(u => u.Username == username)).ReturnsAsync((User)null);

            var result = await _userRepository.GetUserByUsername(username);

            Assert.Null(result);
        }
    }
}
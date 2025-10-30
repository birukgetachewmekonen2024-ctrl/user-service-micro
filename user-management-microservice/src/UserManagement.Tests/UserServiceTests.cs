using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using UserManagement.Service.Services;
using UserManagement.Service.Models;
using UserManagement.Service.Repositories;

namespace UserManagement.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task RegisterUser_ShouldAddUser_WhenUserIsValid()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "testuser", PasswordHash = "hashedpassword", Email = "test@example.com" };
            _userRepositoryMock.Setup(repo => repo.AddUser(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _userService.RegisterUser(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            _userRepositoryMock.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Username = "testuser", PasswordHash = "hashedpassword", Email = "test@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            _userRepositoryMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.Null(result);
            _userRepositoryMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }
    }
}
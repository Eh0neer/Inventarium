using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Inventarium.Context;
using Inventarium.Models;
using Inventarium.Services;

namespace Inventarium.Tests
{
    public class UserServiceTests
    {

        private string _connectionString;
        public UserServiceTests() 
        {
            _connectionString = Constants.DbConnectionStr.MyConnStr;
        }
        [Fact]
        public async Task AddUserAsync_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            using var context = new MyDbContext(options);
            var loggerMock = new Mock<ILogger<UserService>>();
            var userService = new UserService(context, loggerMock.Object);

            var user = new User { UserId = 6, Username = "TestUser", Salt = "some_default_value" };
            var password = "TestPassword";

            // Act
            var result = await userService.AddUserAsync(user, password);

            // Assert
            Assert.True(result);
            Assert.Equal(1, await context.Users.CountAsync());
        }

        [Fact]
        public async Task RemoveUserAsync_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            using var context = new MyDbContext(options);
            var loggerMock = new Mock<ILogger<UserService>>();
            var userService = new UserService(context, loggerMock.Object);

            var user = new User { UserId = 1, Username = "TestUser" };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await userService.RemoveUserAsync(user);

            // Assert
            Assert.True(result);
            Assert.Equal(0, await context.Users.CountAsync());
        }

        [Fact]
        public async Task UpdateUserAsync_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            using var context = new MyDbContext(options);
            var loggerMock = new Mock<ILogger<UserService>>();
            var userService = new UserService(context, loggerMock.Object);

            var user = new User { UserId = 1, Username = "TestUser" };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            user.Username = "UpdatedUsername";

            // Act
            var result = await userService.UpdateUserAsync(user);

            // Assert
            Assert.True(result);
            var updatedUser = await context.Users.FindAsync(user.UserId);
            Assert.Equal("UpdatedUsername", updatedUser.Username);
        }

        [Fact]
        public async Task GetUserAsync_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            using var context = new MyDbContext(options);
            var loggerMock = new Mock<ILogger<UserService>>();
            var userService = new UserService(context, loggerMock.Object);

            var user = new User { UserId = 1, Username = "TestUser" };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await userService.GetUserAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestUser", result.Username);
        }
    }
}

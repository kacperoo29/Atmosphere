using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Atmosphere.Core.Consts;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Services.Auth;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Atmosphere.Services.Tests;

public class JwtTokenProviderTests
{
    private readonly IConfiguration _configuration;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Dictionary<string, string> _jwtSettings = new Dictionary<string, string>
    {
        { "JWT:ExpirationFormats:0", "hh\\:mm\\:ss" },
        { "JWT:Expiration", "01:00:00" },
        { "JWT:Issuer", "Atmosphere" },
        { "JWT:Audience", "Atmosphere" },
        { "JWT:SecretKey", "AtmosphereSecretKey" }
    };

    public JwtTokenProviderTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configuration = new ConfigurationBuilder().AddInMemoryCollection(_jwtSettings).Build();
    }

    [Fact]
    public async Task ValidUserGeneratesToken()
    {
        // Arrange
        var jwtTokenProvider = new JwtTokenProvider(_userRepositoryMock.Object, _configuration);
        var user = User.Create("test", "test");

        // Act
        var result = await jwtTokenProvider.GenerateToken(user);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ValidTokenPassesValidation()
    {
        var jwtTokenProvider = new JwtTokenProvider(_userRepositoryMock.Object, _configuration);
        var user = User.Create("test", "test");
        var token = await jwtTokenProvider.GenerateToken(user);
        _userRepositoryMock.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(user);

        var (valid, _) = await jwtTokenProvider.ValidateToken(token);

        Assert.True(valid);
    }

    [Fact]
    public async Task InvalidTokenFailsValidation()
    {
        var (valid, _) = await new JwtTokenProvider(_userRepositoryMock.Object, _configuration).ValidateToken("invalid token");

        Assert.False(valid);
    }

    [Fact]
    public async Task ValidTokenReturnsClaims()
    {
        var jwtTokenProvider = new JwtTokenProvider(_userRepositoryMock.Object, _configuration);
        var user = User.Create("test", "test");
        var token = await jwtTokenProvider.GenerateToken(user);
        _userRepositoryMock.Setup(x => x.GetUserAsync(user.Id)).ReturnsAsync(user);

        var claims = await jwtTokenProvider.GetClaims(token);

        Assert.NotEmpty(claims);
        Assert.Equal(user.Id.ToString(), claims.Find(x => x.Type == AtmosphereClaimTypes.UserId)?.Value);
    }
}

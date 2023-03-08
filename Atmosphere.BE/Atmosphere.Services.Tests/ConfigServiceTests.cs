namespace Atmosphere.Services.Tests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Atmosphere.Services.Config;
using Atmosphere.Application.Configuration;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Repositories;
using Atmosphere.Core.Validation;
using Atmosphere.Services.Exceptions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class ConfigServiceTests
{
    private readonly Mock<IConfigurationRepository> _configRepo;
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, string> _config = new Dictionary<string, string>()
    {
        { "EmailSettings:SmtpServer", "smtp.gmail.com" },
        { "EmailSettings:SmtpPort", "587" },
        { "EmailSettings:SmtpUsername", "test" },
        { "EmailSettings:SmtpPassword", "test" },
        { "EmailSettings:EmailAddress", "test" },
        { "EmailSettings:ServerEmailAddress", "test" },
    };

    public ConfigServiceTests()
    {
        _configRepo = new Mock<IConfigurationRepository>();
        _configuration = new ConfigurationBuilder().AddInMemoryCollection(_config).Build();
    }

    [Fact]
    public async Task GetNotificationTypesAsync_NotificationTypesKeyExists_ReturnsNotificationTypes()
    {
        // Arrange
        _configRepo
            .Setup(
                x =>
                    x.GetAsync<List<NotificationType>>(
                        ConfigService.NOTIFICATION_TYPES_KEY,
                        default
                    )
            )
            .ReturnsAsync(new List<NotificationType>() { NotificationType.Email, });
        var configService = new ConfigService(_configRepo.Object, _configuration);

        // Act
        var result = await configService.GetNotificationTypesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetNotificationTypesAsync_NotificationTypesKeyDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        var configService = new ConfigService(_configRepo.Object, _configuration);

        // Act
        var result = await configService.GetNotificationTypesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetEmailConfigurationAsync_ConfigurationExists_ReturnsConfiguration()
    {
        var config = new EmailConfiguration
        {
            SmtpServer = "smtp.gmail2.com",
            SmtpPort = 5872,
            SmtpUsername = "test2",
            SmtpPassword = "test2",
            EmailAddress = "test2",
            ServerEmailAddress = "test2",
        };
        _configRepo
            .Setup(x => x.GetAsync<EmailConfiguration>(ConfigService.EMAIL_CONFIG_KEY, default))
            .ReturnsAsync(config);

        var configService = new ConfigService(_configRepo.Object, _configuration);

        var result = await configService.GetEmailConfigurationAsync(default);

        Assert.NotNull(result);
        Assert.Equal(config.SmtpServer, result.SmtpServer);
        Assert.Equal(config.SmtpPort, result.SmtpPort);
        Assert.Equal(config.SmtpUsername, result.SmtpUsername);
        Assert.Equal(config.SmtpPassword, result.SmtpPassword);
        Assert.Equal(config.EmailAddress, result.EmailAddress);
        Assert.Equal(config.ServerEmailAddress, result.ServerEmailAddress);
    }

    [Fact]
    public async Task GetEmailConfigurationAsync_ConfigurationDoesNotExist_ReturnsConfiguration()
    {
        _configRepo
            .Setup(x => x.GetAsync<EmailConfiguration>(ConfigService.EMAIL_CONFIG_KEY, default))
            .ReturnsAsync((EmailConfiguration)null);

        var configService = new ConfigService(_configRepo.Object, _configuration);

        var result = await configService.GetEmailConfigurationAsync(default);

        Assert.NotNull(result);
        Assert.Equal(_config["EmailSettings:SmtpServer"], result.SmtpServer);
        Assert.Equal(int.Parse(_config["EmailSettings:SmtpPort"]), result.SmtpPort);
        Assert.Equal(_config["EmailSettings:SmtpUsername"], result.SmtpUsername);
        Assert.Equal(_config["EmailSettings:SmtpPassword"], result.SmtpPassword);
        Assert.Equal(_config["EmailSettings:EmailAddress"], result.EmailAddress);
        Assert.Equal(_config["EmailSettings:ServerEmailAddress"], result.ServerEmailAddress);
    }

    [Fact]
    public async Task UpdateValidationRules_ValidRuleFormat_UpdatesRules()
    {
        var configService = new ConfigService(_configRepo.Object, _configuration);

        await configService.UpdateValidationRules(
            ReadingType.Temperature,
            new List<ValidationRuleDto>()
            {
                new ValidationRuleDto()
                {
                    Severity = Severity.Error,
                    Condition = "x => x.Value > 0",
                    Message = "Temperature must be greater than 0",
                }
            }
        );
    }

    [Fact]
    public async Task UpdateValidationRules_InvalidRuleFormat_ThrowsException()
    {
        var configService = new ConfigService(_configRepo.Object, _configuration);

        await Assert.ThrowsAsync<InvalidRuleException>(
            async () =>
                await configService.UpdateValidationRules(
                    ReadingType.Temperature,
                    new List<ValidationRuleDto>()
                    {
                        new ValidationRuleDto()
                        {
                            Severity = Severity.Error,
                            Condition = "x => x > 0",
                            Message = "Temperature must be greater than 0",
                        },
                    }
                )
        );
    }

    [Fact]
    public async Task GetValidationRules_RulesDoNotExist_ReturnsEmptyList()
    {
        var configService = new ConfigService(_configRepo.Object, _configuration);

        var result = await configService.GetValidationRules(ReadingType.Temperature);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetValidationRules_RulesExist_ReturnsRules()
    {
        var configService = new ConfigService(_configRepo.Object, _configuration);

        _configRepo
            .Setup(
                x =>
                    x.GetAsync<Dictionary<ReadingType, List<ValidationRule>>>(
                        ReadingValidator.GetValidationRulesKey(null),
                        default
                    )
            )
            .ReturnsAsync(
                new Dictionary<ReadingType, List<ValidationRule>>()
                {
                    {
                        ReadingType.Temperature,
                        new List<ValidationRule>()
                        {
                            new ValidationRule()
                            {
                                Severity = Severity.Error,
                                Condition = x => x.Value > 0,
                                Message = "Temperature must be greater than 0",
                            },
                        }
                    },
                }
            );

        var result = await configService.GetValidationRules(ReadingType.Temperature);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
}

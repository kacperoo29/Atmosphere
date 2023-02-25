using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Core.Validation;
using Moq;
using Xunit;

namespace Atmosphere.Core.Tests;

public class ValidatorTests
{
    private readonly Mock<IConfigurationRepository> _configurationRepositoryMock;

    public ValidatorTests()
    {
        _configurationRepositoryMock = new Mock<IConfigurationRepository>();
        var validationRules = new Dictionary<ReadingType, List<ValidationRule>>()
        {
            {
                ReadingType.Temperature,
                new List<ValidationRule>
                {
                    new ValidationRule
                    {
                        Message = "Temperature is below {severity} threshold of {value}",
                        Severity = Severity.Warning,
                        Condition = v => v.Value < 0
                    },
                    new ValidationRule
                    {
                        Message = "Temperature is above {severity} threshold of {value}",
                        Severity = Severity.Warning,
                        Condition = v => v.Value > 100
                    },
                    new ValidationRule
                    {
                        Message = "Temperature is below {severity} threshold of {value}",
                        Severity = Severity.Error,
                        Condition = v => v.Value < -10
                    },
                    new ValidationRule
                    {
                        Message = "Temperature is above {severity} threshold of {value}",
                        Severity = Severity.Error,
                        Condition = v => v.Value > 110
                    }
                }
            }
        };
        _configurationRepositoryMock
            .Setup(
                x =>
                    x.GetAsync<Dictionary<ReadingType, List<ValidationRule>>>(
                        It.IsAny<string>(),
                        CancellationToken.None
                    )
            )
            .ReturnsAsync(validationRules);
    }

    [Fact]
    public async Task NoErrorsWhenValueIsWithinThreshold()
    {
        var validator = new ReadingValidator(_configurationRepositoryMock.Object);
        var reading = Reading.Create(Guid.NewGuid(), 50, "C", DateTime.UtcNow, ReadingType.Temperature);
        var notifications = await validator.Validate(reading);
        Assert.Empty(notifications);
    }

    [Fact]
    public async Task WarningWhenValueIsBelowWarningThreshold()
    {
        var validator = new ReadingValidator(_configurationRepositoryMock.Object);
        var reading = Reading.Create(Guid.NewGuid(), -5, "C", DateTime.UtcNow, ReadingType.Temperature);
        var notifications = await validator.Validate(reading);
        Assert.NotEmpty(notifications);
        Assert.Single(notifications);
        Assert.Equal(Severity.Warning, notifications.First().Severity);
        Assert.Equal("Temperature is below Warning threshold of -5", notifications.First().Message);
    }

    [Fact]
    public async Task WarningWhenValueIsAboveWarningThreshold()
    {
        var validator = new ReadingValidator(_configurationRepositoryMock.Object);
        var reading = Reading.Create(Guid.NewGuid(), 105, "C", DateTime.UtcNow, ReadingType.Temperature);
        var notifications = await validator.Validate(reading);
        Assert.NotEmpty(notifications);
        Assert.Single(notifications);
        Assert.Equal(Severity.Warning, notifications.First().Severity);
        Assert.Equal("Temperature is above Warning threshold of 105", notifications.First().Message);
    }

    [Fact]
    public async Task ErrorWhenValueIsBelowErrorThreshold()
    {
        var validator = new ReadingValidator(_configurationRepositoryMock.Object);
        var reading = Reading.Create(Guid.NewGuid(), -15, "C", DateTime.UtcNow, ReadingType.Temperature);
        var notifications = await validator.Validate(reading);
        Assert.NotEmpty(notifications);
        Assert.Single(notifications);
        Assert.Equal(Severity.Error, notifications.First().Severity);
        Assert.Equal("Temperature is below Error threshold of -15", notifications.First().Message);
    }

    [Fact]
    public async Task ErrorWhenValueIsAboveErrorThreshold()
    {
        var validator = new ReadingValidator(_configurationRepositoryMock.Object);
        var reading = Reading.Create(Guid.NewGuid(), 115, "C", DateTime.UtcNow, ReadingType.Temperature);
        var notifications = await validator.Validate(reading);
        Assert.NotEmpty(notifications);
        Assert.Single(notifications);
        Assert.Equal(Severity.Error, notifications.First().Severity);
        Assert.Equal("Temperature is above Error threshold of 115", notifications.First().Message);
    }

    [Fact]
    public async Task NoRulesReturnedDefaultsCreated()
    {
        _configurationRepositoryMock
            .Setup(
                x =>
                    x.GetAsync<Dictionary<ReadingType, List<ValidationRule>>>(
                        It.IsAny<string>(),
                        CancellationToken.None
                    )
            )
            .ReturnsAsync((Dictionary<ReadingType, List<ValidationRule>>)null);
        
        var validator = new ReadingValidator(_configurationRepositoryMock.Object);
        var rules = validator.GetDefaultRules();
        var reading = Reading.Create(Guid.NewGuid(), 115, "C", DateTime.UtcNow, ReadingType.Temperature);
        var notifications = await validator.Validate(reading);
        Assert.NotEmpty(notifications);
        Assert.Single(notifications);
        Assert.Equal(Severity.Error, notifications.First().Severity);
    }

    [Fact]
    public async Task NoRulesForReadingTypeReturnsEmptyResult()
    {
        var validator = new ReadingValidator(_configurationRepositoryMock.Object);
        var reading = Reading.Create(Guid.NewGuid(), 115, "C", DateTime.UtcNow, ReadingType.Humidity);
        var notifications = await validator.Validate(reading);
        Assert.Empty(notifications);
    }
}

using BedeLottery.Common.Configurations;
using BedeLottery.Models.Validators;
using FluentAssertions;

namespace BedeLottery.Tests.Validators;

public class LotterySettingsValidatorTests
{
    [Test]
    public void LotterySettingsValidator_ValidSettings_Passes()
    {
        // Arrange
        var settings = new LotterySettings
        {
            MinPlayers = 10,
            MaxPlayers = 15,
            MinTicketsPerPlayer = 1,
            MaxTicketsPerPlayer = 10,
            TicketPrice = 1m,
            GrandPrizePercentage = 0.5m,
            SecondTierPoolPercentage = 0.3m,
            ThirdTierPoolPercentage = 0.1m,
            SecondTierTicketPercentage = 0.1m,
            ThirdTierTicketPercentage = 0.2m
        };

        // Act
        var validator = new LotterySettingsValidator();
        var result = validator.Validate(settings);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void LotterySettingsValidator_InvalidPercentages_Fails()
    {
        // Arrange
        var settings = new LotterySettings
        {
            MinPlayers = 10,
            MaxPlayers = 9,
            MinTicketsPerPlayer = 1,
            MaxTicketsPerPlayer = 10,
            TicketPrice = 1m,
            GrandPrizePercentage = 0.8m,
            SecondTierPoolPercentage = 0.3m,
            ThirdTierPoolPercentage = 0.1m,
            SecondTierTicketPercentage = 0.1m,
            ThirdTierTicketPercentage = 0.2m
        };

        // Act
        var validator = new LotterySettingsValidator();
        var result = validator.Validate(settings);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}

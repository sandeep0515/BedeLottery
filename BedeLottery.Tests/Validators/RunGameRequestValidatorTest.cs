using BedeLottery.Common.Configurations;
using BedeLottery.Common.Enum;
using BedeLottery.Models;
using BedeLottery.Models.Validators;
using FluentAssertions;

namespace BedeLottery.Tests.Validators;

public class RunGameRequestValidatorTest
{

    [Test]
    public void RunGameRequestValidator_ValidRequest_Success()
    {
        // Arrange
        var settings = new LotterySettings { MinPlayers = 2, MaxPlayers = 5, MinTicketsPerPlayer = 1, MaxTicketsPerPlayer = 10 };
        var game = new Game { GameId = 1 };
        List<Player> players = [];
        for (int i = 1; i <= 2; i++)
        {
            var p = new Player { Id = i, Name = $"Player {i}", PlayerType = i == 1 ? PlayerType.Human : PlayerType.CPU };
            p.PlayerGames.Add(new PlayerGame { GameId = game.GameId, PlayerId = p.Id, TicketsBought = 1 });
            players.Add(p);
        }

        var req = new RunGameRequest { Game = game, Players = players, Settings = settings };

        // Act
        var validator = new RunGameRequestValidator();
        var result = validator.Validate(req);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void RunGameRequestValidator_InvalidRequest_Fails()
    {
        // Arrange
        var settings = new LotterySettings { MinPlayers = 3, MaxPlayers = 5, MinTicketsPerPlayer = 1, MaxTicketsPerPlayer = 10 };
        var game = new Game { GameId = 2 };
        var players = new List<Player>();
        for (int i = 1; i <= 2; i++)
        {
            var p = new Player { Id = i, Name = $"Player {i}", PlayerType = PlayerType.CPU };
            p.PlayerGames.Add(new PlayerGame { GameId = game.GameId, PlayerId = p.Id, TicketsBought = 1 });
            players.Add(p);
        }

        var req = new RunGameRequest { Game = game, Players = players, Settings = settings };

        // Act
        var validator = new RunGameRequestValidator();
        var result = validator.Validate(req);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}

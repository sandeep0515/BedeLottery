using BedeLottery.Common.Configurations;
using BedeLottery.Common.Enum;
using BedeLottery.Models;
using BedeLottery.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace BedeLottery.Tests.Services;

public class LotteryEngineTests
{
    private LotteryGameEngineService _engine;
    private LotterySettings _settings;

    [SetUp]
    public void Setup()
    {
        _settings = new LotterySettings
        {
            MinPlayers = 10,
            MaxPlayers = 15,
            MinTicketsPerPlayer = 1,
            MaxTicketsPerPlayer = 10,
            StartingBalance = 10m,
            TicketPrice = 1m,
            GrandPrizePercentage = 0.5m,
            SecondTierPoolPercentage = 0.3m,
            ThirdTierPoolPercentage = 0.1m,
            SecondTierTicketPercentage = 0.10m,
            ThirdTierTicketPercentage = 0.20m
        };
        var logger = NullLogger<LotteryGameEngineService>.Instance;
        _engine = new LotteryGameEngineService(_settings, logger);
    }

    [Test]
    public void RunGame_NoTickets_ReturnsNoRevenue()
    {
        // Act
        var game = _engine.CreateGame(1);
        List<Player> players = [];

        // Act
        var result = _engine.RunGame(game, players);

        // Assert
        result.HouseProfit.Should().Be(0m);
        result.PrizeTiers.Should().BeEmpty();
    }

    [Test]
    public void RunGame_InitilisePlayers_AllocatesPrizesAndHouseProfit()
    {
        // Act
        var game = _engine.CreateGame(1);
        List<Player> players = [];
        for (int i = 1; i <= 10; i++)
        {
            var p = new Player { Id = i, Name = $"Player {i}", PlayerType = i == 1 ? PlayerType.Human : PlayerType.CPU, Balance = 10m };
            p.PlayerGames.Add(new PlayerGame { GameId = game.GameId, PlayerId = p.Id, TicketsBought = 1 });
            players.Add(p);
        }

        // Act
        var result = _engine.RunGame(game, players);

        // Assert
        result.Players.Should().HaveCount(10);
        result.PrizeTiers.Should().HaveCount(3);
        result.HouseProfit.Should().BeGreaterThanOrEqualTo(0m);
    }
}

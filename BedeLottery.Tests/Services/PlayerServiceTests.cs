using BedeLottery.Common.Enum;
using BedeLottery.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace BedeLottery.Tests.Services;

public class PlayerServiceTests
{
    private PlayerService _service;

    [SetUp]
    public void Setup()
    {
        var logger = NullLogger<PlayerService>.Instance;
        _service = new PlayerService(logger);
    }

    [Test]
    public void CreatePlayer_ShouldReturnPlayerWithGivenValues()
    {
        // Arrange
        int playerId = 5;
        string playerName = "Player 1";
        PlayerType playerType = PlayerType.CPU;

        // Act
        var p = _service.CreatePlayer(playerId, playerName, playerType);

        // Assert
        p.Should().NotBeNull();
        p.Id.Should().Be(5);
        p.Name.Should().Be("Player 1");
        p.PlayerType.Should().Be(PlayerType.CPU);
        p.PlayerGames.Should().BeEmpty();
    }

    [Test]
    public void AddPlayerGameRecord_ShouldAddRecordToPlayer()
    {
        // Arrange
        int playerId = 1;
        string playerName = "Player 1";

        // Act
        var player = _service.CreatePlayer(playerId, playerName);
        _service.AddPlayerGameRecord(player, 42, 3);

        // Assert
        player.PlayerGames.Should().HaveCount(1);
        var playerGame = player.PlayerGames.First();
        playerGame.GameId.Should().Be(42);
        playerGame.PlayerId.Should().Be(player.Id);
        playerGame.TicketsBought.Should().Be(3);
    }

    [Test]
    public void CreateOrLoadPlayer_ShouldCreatePlayerAndAddGameRecord()
    {
        // Arrange
        int playerId = 2;
        string playerName = "Player 1";
        int gameId = 7;
        int ticketsBought = 4;
        PlayerType playerType = PlayerType.Human;

        // Act
        var player = _service.CreateOrLoadPlayer(playerId,playerName,gameId,ticketsBought,playerType);

        // Assert
        player.Should().NotBeNull();
        player.Id.Should().Be(2);
        player.Name.Should().Be("Player 1");
        player.PlayerGames.Should().HaveCount(1);
        player.PlayerGames.First().GameId.Should().Be(7);
        player.PlayerGames.First().TicketsBought.Should().Be(4);
    }
}

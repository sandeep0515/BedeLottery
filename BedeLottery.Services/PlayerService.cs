using BedeLottery.Common.Enum;
using BedeLottery.Common.Utilities;
using BedeLottery.Models;
using BedeLottery.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BedeLottery.Services;

public class PlayerService : IPlayerService
{
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(ILogger<PlayerService> logger)
    {
        _logger = logger;
    }

    public Player CreatePlayer(int id, string name, PlayerType playerType = PlayerType.CPU)
    {
        return new Player
        {
            Id = id,
            Name = name,
            PlayerType = playerType
        };
    }

    public void AddPlayerGameRecord(Player player, int gameId, int ticketsBought)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(player);
            player.PlayerGames.Add(new PlayerGame
            {
                GameId = gameId,
                PlayerId = player.Id,
                TicketsBought = ticketsBought
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add player game record for player {PlayerId}", player?.Id);
            throw;
        }
    }

    public Player CreateOrLoadPlayer(int id, string name, int gameId, int ticketsBought, PlayerType playerType = PlayerType.CPU)
    {
        var player = CreatePlayer(id, name, playerType);
        AddPlayerGameRecord(player, gameId, ticketsBought);
        return player;
    }

    public List<Player> InitialisePlayers(int gameId, int ticketCount)
    {
        List<Player> players = [];
        try
        {
            var validTicketCount = Math.Clamp(ticketCount, 1, 10);
            var user = CreateOrLoadPlayer(1, "Player 1", gameId, validTicketCount, PlayerType.Human);
            user.Balance = 10m - validTicketCount;
            user.TotalSpent = validTicketCount;
            players.Add(user);

            var totalPlayers = RandomGenerator.GenerateRandomNumber(10, 16);
            _logger.LogInformation("Initializing {Count} players", totalPlayers);
            for (int i = 2; i <= totalPlayers; i++)
            {
                var tickets = RandomGenerator.GenerateRandomNumber(1, 11);
                var cpu = CreateOrLoadPlayer(i, $"Player {i}", gameId, tickets, PlayerType.CPU);
                cpu.Balance = 10m - tickets;
                cpu.TotalSpent = tickets;
                players.Add(cpu);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize players for game {GameId}", gameId);
            throw;
        }

        return players;
    }
}

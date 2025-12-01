using BedeLottery.Common.Enum;
using BedeLottery.Models;
using BedeLottery.Services.Interfaces;

namespace BedeLottery.Services;

public class PlayerService : IPlayerService
{

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
        ArgumentNullException.ThrowIfNull(player);
        player.PlayerGames.Add(new PlayerGame
        {
            GameId = gameId,
            PlayerId = player.Id,
            TicketsBought = ticketsBought
        });
    }

    public Player CreateOrLoadPlayer(int id, string name, int gameId, int ticketsBought, PlayerType playerType = PlayerType.CPU)
    {
        var player = CreatePlayer(id, name, playerType);
        AddPlayerGameRecord(player, gameId, ticketsBought);
        return player;
    }
}

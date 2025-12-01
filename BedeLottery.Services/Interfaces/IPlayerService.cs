using BedeLottery.Common.Enum;
using BedeLottery.Models;

namespace BedeLottery.Services.Interfaces;

public interface IPlayerService
{
    Player CreatePlayer(int id, string name, PlayerType playerType = PlayerType.CPU);
    void AddPlayerGameRecord(Player player, int gameId, int ticketsBought);
    Player CreateOrLoadPlayer(int id, string name, int gameId, int ticketsBought, PlayerType playerType = PlayerType.CPU);
}

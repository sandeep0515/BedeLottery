using BedeLottery.Models;
using BedeLottery.Models.Response;

namespace BedeLottery.Services.Interfaces;

public interface ILotteryGameEngineService
{
    LotteryGameResult RunGame(Game game, List<Player> players);
    List<Ticket> GenerateTickets(Game game, List<Player> players);
    Game CreateGame(int gameId);
}

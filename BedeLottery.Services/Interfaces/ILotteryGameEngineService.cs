using BedeLottery.Models;
using BedeLottery.Models.Response;

namespace BedeLottery.Services.Interfaces;

public interface ILotteryGameEngineService
{
    LotteryGameResult RunGame(int gameId, int numberOfTicketsPurchased);

    Game CreateGame(int gameId);
}

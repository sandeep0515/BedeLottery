using BedeLottery.Models;
using BedeLottery.Models.Response;
using BedeLottery.Services.Interfaces;

namespace BedeLottery.Services;

public class LotteryGameEngineService(IPlayerService playerService): ILotteryGameEngineService
{
    private readonly IPlayerService _playerService = playerService;

    private Dictionary<int, List<Player>> _registeredPlayers = []; // In-memory store to hold registered players for the game

    public Game CreateGame(int gameId)
    {
        return new()
        {
            GameId = gameId,
            PlayedOn = DateTime.UtcNow
        };
    }

    public LotteryGameResult RunGame(int gameId, int numberOfTicketsPurchased)
    {
        var game = CreateGame(gameId);

        var playerOne = _playerService.CreateOrLoadPlayer(1, "Player 1", gameId, numberOfTicketsPurchased, Common.Enum.PlayerType.Human);




        return new();
    }



    
}

using BedeLottery.Common.Enum;

namespace BedeLottery.Models;

public class PlayerGame
{
    public int GameId { get; set; }
    public int PlayerId { get; set; }
    public int TicketsBought { get; set; }
    public decimal TotalSpent { get; set; }
    public GameResultType? ResultType { get; set; }
    public decimal TotalAmountWon { get; set; }
    public int TotalWinningTickets { get; set; }
}

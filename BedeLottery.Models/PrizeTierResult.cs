using BedeLottery.Common.Enum;

namespace BedeLottery.Models;

public class PrizeTierResult
{
    public required PrizeTierType PrizeTierResultType { get; set; }
    public decimal PrizePerTicket { get; set; }
    public List<Ticket> WinningTickets { get; set; } = [];
}

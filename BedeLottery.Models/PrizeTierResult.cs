namespace BedeLottery.Models;

public class PrizeTierResult
{
    public required PrizeTierResult PrizeTierResultType { get; set; }
    public decimal PrizePerTicket { get; set; }
    public List<Ticket> WinningTickets { get; set; } = [];
}

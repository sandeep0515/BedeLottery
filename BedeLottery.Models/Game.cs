namespace BedeLottery.Models;

public class Game
{
    public int GameId { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal HouseProfit { get; set; }
    public DateTime PlayedOn { get; set; }
    public List<PrizeTierResult> PrizeTierResults { get; set; } = [];
}

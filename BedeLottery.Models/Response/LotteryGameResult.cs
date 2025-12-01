namespace BedeLottery.Models.Response;

public class LotteryGameResult
{
    public List<Player> Players { get; set; } = [];
    public List<PrizeTierResult> PrizeTiers { get; set; } = [];
    public decimal HouseProfit { get; set; }
}

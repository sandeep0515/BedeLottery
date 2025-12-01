namespace BedeLottery.Common.Configurations;

public class LotterySettings
{
    public int MinPlayers { get; set; } = 10;
    public int MaxPlayers { get; set; } = 15;
    public int MinTicketsPerPlayer { get; set; } = 1;
    public int MaxTicketsPerPlayer { get; set; } = 10;
    public decimal StartingBalance { get; set; } = 10m;
    public decimal TicketPrice { get; set; } = 1m;
    public decimal GrandPrizePercentage { get; set; } = 0.5m;
    public decimal SecondTierPoolPercentage { get; set; } = 0.30m;
    public decimal ThirdTierPoolPercentage { get; set; } = 0.10m;
    // Ticket win percentage of total tickets per tier (e.g., 0.10 = 10% of tickets win in second tier)
    public decimal SecondTierTicketPercentage { get; set; } = 0.10m;
    public decimal ThirdTierTicketPercentage { get; set; } = 0.20m;
}

using BedeLottery.Common.Enum;

namespace BedeLottery.Models;

public class Player
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public PlayerType PlayerType { get; set; }
    public decimal Balance { get; set; }
    public decimal TotalSpent { get; set; }
    public List<PlayerGame> PlayerGames { get; set; } = [];
}

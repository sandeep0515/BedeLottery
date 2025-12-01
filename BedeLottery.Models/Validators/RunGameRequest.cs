using BedeLottery.Common.Configurations;

namespace BedeLottery.Models.Validators;

public class RunGameRequest
{
    public required Game Game { get; init; }
    public required List<Player> Players { get; init; }
    public required LotterySettings Settings { get; init; }
}

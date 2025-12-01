using BedeLottery.Common.Configurations;
using FluentValidation;

namespace BedeLottery.Models.Validators;

public class LotterySettingsValidator : AbstractValidator<LotterySettings>
{
    public LotterySettingsValidator()
    {
        RuleFor(x => x.MinPlayers)
            .GreaterThan(0).WithMessage("MinPlayers must be > 0");

        RuleFor(x => x.MaxPlayers)
            .GreaterThanOrEqualTo(x => x.MinPlayers).WithMessage("MaxPlayers must be >= MinPlayers");

        RuleFor(x => x.MinTicketsPerPlayer)
            .GreaterThan(0).WithMessage("MinTicketsPerPlayer must be > 0");

        RuleFor(x => x.MaxTicketsPerPlayer)
            .GreaterThanOrEqualTo(x => x.MinTicketsPerPlayer).WithMessage("MaxTicketsPerPlayer must be >= MinTicketsPerPlayer");

        RuleFor(x => x.TicketPrice)
            .GreaterThan(0).WithMessage("TicketPrice must be > 0");

        RuleFor(x => x.GrandPrizePercentage)
            .InclusiveBetween(0m, 1m).WithMessage("GrandPrizePercentage must be between 0 and 1");

        RuleFor(x => x.SecondTierPoolPercentage)
            .InclusiveBetween(0m, 1m).WithMessage("SecondTierPoolPercentage must be between 0 and 1");

        RuleFor(x => x.ThirdTierPoolPercentage)
            .InclusiveBetween(0m, 1m).WithMessage("ThirdTierPoolPercentage must be between 0 and 1");

        RuleFor(x => x.SecondTierTicketPercentage)
            .InclusiveBetween(0m, 1m).WithMessage("SecondTierTicketPercentage must be between 0 and 1");

        RuleFor(x => x.ThirdTierTicketPercentage)
            .InclusiveBetween(0m, 1m).WithMessage("ThirdTierTicketPercentage must be between 0 and 1");

        RuleFor(x => x)
            .Must(x => x.GrandPrizePercentage + x.SecondTierPoolPercentage + x.ThirdTierPoolPercentage <= 1.0m)
            .WithMessage("Sum of prize pool percentages must be <= 1.0");
    }
}

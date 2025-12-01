using FluentValidation;
using System.Linq;
using BedeLottery.Common.Configurations;
using BedeLottery.Models;

namespace BedeLottery.Models.Validators;

public class RunGameRequestValidator : AbstractValidator<RunGameRequest>
{
    public RunGameRequestValidator()
    {
        RuleFor(x => x.Game)
            .NotNull()
            .WithMessage("Game is required.");
        RuleFor(x => x.Players)
            .NotNull()
            .Must((root, players) => players != null && players.Count >= root.Settings.MinPlayers && players.Count <= root.Settings.MaxPlayers)
            .WithMessage(root => $"Total players must be between {root.Settings.MinPlayers} and {root.Settings.MaxPlayers}.");

        RuleForEach(x => x.Players)
            .Must((root, player) =>
            {
                var pg = player.PlayerGames?.FirstOrDefault(g => g.GameId == root.Game.GameId);
                if (pg == null) return false;
                return pg.TicketsBought >= root.Settings.MinTicketsPerPlayer && pg.TicketsBought <= root.Settings.MaxTicketsPerPlayer;
            })
            .WithMessage(root => $"Each player's tickets for game {root.Game.GameId} must be between {root.Settings.MinTicketsPerPlayer} and {root.Settings.MaxTicketsPerPlayer}.");
    }
 
}

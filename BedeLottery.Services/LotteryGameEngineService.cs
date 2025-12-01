using BedeLottery.Common.Configurations;
using BedeLottery.Common.Enum;
using BedeLottery.Common.Utilities;
using BedeLottery.Models;
using BedeLottery.Models.Response;
using BedeLottery.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BedeLottery.Services;

public class LotteryGameEngineService(LotterySettings settings, ILogger<LotteryGameEngineService> logger) : ILotteryGameEngineService
{
    private readonly LotterySettings _settings = settings ?? new LotterySettings();
    private readonly ILogger<LotteryGameEngineService> _logger = logger;

    public Game CreateGame(int gameId)
    {
        return new()
        {
            GameId = gameId,
            PlayedOn = DateTime.UtcNow
        };
    }

    public LotteryGameResult RunGame(Game game, List<Player> players)
    {
        _logger.LogInformation("Running game {GameId} with {PlayerCount} players", game.GameId, players?.Count ?? 0);
        // Generate Tickets
        List<Ticket> tickets;
        try
        {
            tickets = GenerateTickets(game, players);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate tickets for game {GameId}", game.GameId);
            throw;
        }
        var totalTickets = tickets.Count;

        if (totalTickets == 0)
        {
            game.TotalRevenue = 0m;
            return new LotteryGameResult { Players = players, PrizeTiers = [], HouseProfit = 0m };
        }

        game.TotalRevenue = totalTickets * _settings.TicketPrice;

        // Read settings for prize calculations
        var grandPercentage = _settings.GrandPrizePercentage;
        var secondPoolPercentage = _settings.SecondTierPoolPercentage;
        var thirdPoolPercentage = _settings.ThirdTierPoolPercentage;
        var secondTierTickets = (int)Math.Floor(totalTickets * _settings.SecondTierTicketPercentage);
        var thirdTierTickets = (int)Math.Floor(totalTickets * _settings.ThirdTierTicketPercentage);

        // Minimum one winner per tier if possible
        if (secondTierTickets == 0 && totalTickets > 1) secondTierTickets = 1;
        if (thirdTierTickets == 0 && totalTickets > secondTierTickets + 1) thirdTierTickets = 1;

        var secondTierPoolAmount = Math.Floor(game.TotalRevenue * secondPoolPercentage * 100) / 100;
        var thirdTierPoolAmount = Math.Floor(game.TotalRevenue * thirdPoolPercentage * 100) / 100;
        var grandPrizeAmount = Math.Floor(game.TotalRevenue * grandPercentage * 100) / 100;

        List<PrizeTierResult> prizeResults = [];

        // Prize Tier Results
        prizeResults.Add(CreatePrizeTierResult(tickets, 1, grandPrizeAmount, PrizeTierType.Grand));
        prizeResults.Add(CreatePrizeTierResult(tickets, Math.Min(secondTierTickets, tickets.Count), secondTierPoolAmount, PrizeTierType.Second));
        prizeResults.Add(CreatePrizeTierResult(tickets, Math.Min(thirdTierTickets, tickets.Count), thirdTierPoolAmount, PrizeTierType.Third));

        try
        {
            foreach (var tier in prizeResults)
            {
                foreach (var ticket in tier.WinningTickets)
                {
                    var player = players.FirstOrDefault(p => p.Id == ticket.PlayerId);
                    if (player is null) continue;

                    var playerGameRecord = player.PlayerGames.FirstOrDefault(pg => pg.GameId == game.GameId);
                    if (playerGameRecord is null) continue;

                    playerGameRecord.TotalWinningTickets += 1;
                    playerGameRecord.TotalAmountWon += tier.PrizePerTicket;
                    player.Balance += tier.PrizePerTicket;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to apply payouts for game {GameId}", game.GameId);
            throw;
        }

        var totalPaidOut = prizeResults.Sum(pr => pr.PrizePerTicket * pr.WinningTickets.Count);
        var houseProfit = Math.Round(game.TotalRevenue - totalPaidOut, 2);
        game.HouseProfit = houseProfit;
        game.PrizeTierResults = prizeResults;

        return new LotteryGameResult
        {
            Players = players,
            PrizeTiers = prizeResults,
            HouseProfit = houseProfit
        };
    }

    public List<Ticket> GenerateTickets(Game game, List<Player> players)
    {
        List<Ticket> tickets = [];
        var ticketNumber = 1;
        foreach (var player in players)
        {
            var playerGameRecord = player.PlayerGames.FirstOrDefault(pg => pg.GameId == game.GameId);
            var ticketsBought = playerGameRecord?.TicketsBought ?? 0;

            for (int i = 0; i < ticketsBought; i++)
            {
                tickets.Add(new Ticket { TicketNumber = ticketNumber++, PlayerId = player.Id });
            }
        }
        return tickets;
    }

    private static PrizeTierResult CreatePrizeTierResult(List<Ticket> pool, int winnersCount, decimal prizePoolAmount, PrizeTierType tierType)
    {
        var winners = DrawRandomTickets(pool, winnersCount);
        foreach (var winner in winners)
        {
            pool.RemoveAll(x => x.TicketNumber == winner.TicketNumber);
        }

        var prizePerTicket = winners.Count > 0 ? Math.Floor((prizePoolAmount / winners.Count) * 100) / 100 : 0m;

        return new PrizeTierResult
        {
            PrizeTierResultType = tierType,
            PrizePerTicket = prizePerTicket,
            WinningTickets = winners
        };
    }

    private static List<Ticket> DrawRandomTickets(List<Ticket> pool, int count)
    {
        List<Ticket> winners = [];
        var available = pool.ToList();
        for (int i = 0; i < count && available.Count > 0; i++)
        {
            var index = RandomGenerator.GenerateRandomNumber(available.Count);
            winners.Add(available[index]);
            available.RemoveAt(index);
        }
        return winners;
    }
}

using BedeLottery.Common.Configurations;
using BedeLottery.Common.Enum;
using BedeLottery.Models;
using BedeLottery.Models.Response;
using BedeLottery.Models.Validators;
using BedeLottery.Services;
using BedeLottery.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

static LotterySettings LoadSettings()
{
    var settings = new LotterySettings();
    var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
    try
    {
        var json = File.Exists(settingsPath) ? File.ReadAllText(settingsPath) : string.Empty;
        using var doc = JsonDocument.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json);
        settings = doc.RootElement.TryGetProperty("LotterySettings", out var root)
            ? JsonSerializer.Deserialize<LotterySettings>(root.GetRawText()) ?? new()
            : new();
    }
    catch
    {
        settings = new();
    }
    return settings;
}

static ServiceProvider ConfigureServices(LotterySettings settings)
{
    var services = new ServiceCollection();
    services.AddSingleton(settings);
    services.AddSingleton<IValidator<LotterySettings>, LotterySettingsValidator>();
    services.AddSingleton<IValidator<RunGameRequest>, RunGameRequestValidator>();
    services.AddTransient<IPlayerService, PlayerService>();
    services.AddTransient<ILotteryGameEngineService, LotteryGameEngineService>();
    return services.BuildServiceProvider();
}

static int PromptTicketCount(LotterySettings settings)
{
    Console.Write("How many tickets do you want to buy, Player 1?");
    var input = Console.ReadLine();
    if (!int.TryParse(input, out var tickets)) tickets = 1;
    return Math.Clamp(tickets, settings.MinTicketsPerPlayer, settings.MaxTicketsPerPlayer);
}

static (Game game, List<Player> players) PrepareGame(IPlayerService playerService, int tickets)
{
    var game = new Game { GameId = 1, PlayedOn = DateTime.UtcNow };
    var players = playerService.InitialisePlayers(game.GameId, tickets);
    return (game, players);
}

static bool ValidateRun(ServiceProvider provider, Game game, List<Player> players, LotterySettings settings)
{
    var runRequest = new RunGameRequest { Game = game, Players = players, Settings = settings };
    var runValidator = provider.GetRequiredService<IValidator<RunGameRequest>>();
    var runResult = runValidator.Validate(runRequest);
    if (!runResult.IsValid)
    {
        Console.WriteLine("Invalid run configuration:");
        foreach (var err in runResult.Errors)
        {
            Console.WriteLine($" - {err.ErrorMessage}");
        }
        return false;
    }
    return true;
}

static void PrintResults(LotteryGameResult result)
{
    Console.WriteLine();
    Console.WriteLine("Ticket Draw Results:");
    Console.WriteLine();

    foreach (var tier in result.PrizeTiers)
    {
        var parts = tier.WinningTickets.GroupBy(w => w.PlayerId).Select(i => $"{i.Key}({i.Count()})");
        Console.WriteLine($"* {tier.PrizeTierResultType} : Players {string.Join(", ", parts)} win ${tier.PrizePerTicket:F2} per winning ticket!");
    }

    Console.WriteLine();
    Console.WriteLine($"House profit: ${result.HouseProfit}");
}


var settings = LoadSettings();
var provider = ConfigureServices(settings);
var engine = provider.GetRequiredService<ILotteryGameEngineService>();
var playerService = provider.GetRequiredService<IPlayerService>();

Console.WriteLine("Welcome to the Bede Lottery, Player 1!");
Console.WriteLine();
Console.WriteLine($"* Your digital balance: ${settings.StartingBalance:F2}");
Console.WriteLine($"* Ticket Price: ${settings.TicketPrice:F2} each");
Console.WriteLine();

var tickets = PromptTicketCount(settings);
var (game, players) = PrepareGame(playerService, tickets);

var cpuCount = players.Count(p => p.PlayerType == PlayerType.CPU);
Console.WriteLine($"{cpuCount} other CPU players also have purchased tickets");

if (!ValidateRun(provider, game, players, settings)) return;

var result = engine.RunGame(game, players);

PrintResults(result);




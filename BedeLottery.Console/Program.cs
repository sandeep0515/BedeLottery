using BedeLottery.Services;
using BedeLottery.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();
services.AddTransient<ILotteryGameEngineService, LotteryGameEngineService>();

Console.WriteLine("Welcome to the Bede Lottery, Player!");



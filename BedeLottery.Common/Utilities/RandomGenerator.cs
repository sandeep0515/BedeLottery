namespace BedeLottery.Common.Utilities;

public static class RandomGenerator
{
   public static int GenerateRandomNumber(int min, int max) => Random.Shared.Next(min, max);
}

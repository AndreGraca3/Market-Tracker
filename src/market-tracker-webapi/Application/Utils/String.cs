using System.Diagnostics.CodeAnalysis;

namespace market_tracker_webapi.Application.Utils;

[ExcludeFromCodeCoverage]
public static class RandomStringGenerator
{
    public static string GenerateRandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    public static string CentsToEuros(this int cents)
    {
        var euros = (double)cents / 100;
        return euros.ToString("0.00");
    }
}
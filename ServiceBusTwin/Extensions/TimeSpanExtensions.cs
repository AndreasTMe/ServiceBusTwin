namespace ServiceBusTwin.Extensions;

internal static class TimeSpanExtensions
{
    public static string ToIso8601String(this TimeSpan timeSpan)
    {
        var isoString = "P";

        if (timeSpan.Days > 0)
        {
            var days = timeSpan.Days;

            var years = days / 365;
            if (years > 0)
            {
                isoString += $"{years}Y";
                days      %= 365;
            }

            var months = days / 12;
            if (months > 0)
            {
                isoString += $"{months}M";
                days      %= 12;
            }

            if (days > 0)
            {
                isoString += $"{days}D";
            }
        }

        if (timeSpan.Hours > 0 || timeSpan.Minutes > 0 || timeSpan.Seconds > 0 || timeSpan.Milliseconds > 0)
        {
            isoString += "T";

            if (timeSpan.Hours > 0)
            {
                isoString += $"{timeSpan.Hours}H";
            }

            if (timeSpan.Minutes > 0)
            {
                isoString += $"{timeSpan.Minutes}M";
            }

            if (timeSpan.Seconds > 0)
            {
                isoString += $"{timeSpan.Seconds}S";
            }
        }

        return isoString;
    }
}
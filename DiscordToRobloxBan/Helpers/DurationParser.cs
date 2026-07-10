

using System;
using System.Text.RegularExpressions;

namespace DiscordToRobloxBan.Helpers;

    public static class DurationParser
    {
        private static readonly Regex DurationRegex = new(@"(\d+)\s*([wdhms])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        public static bool TryParse(string input, out TimeSpan result)
        {
            result = TimeSpan.Zero;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            var matches = DurationRegex.Matches(input);
            if (matches.Count == 0)
                return false;

            double totalSeconds = 0;

            foreach (Match match in matches)
            {
                if (!long.TryParse(match.Groups[1].Value, out long value))
                    return false;

                string unit = match.Groups[2].Value.ToLower();

                totalSeconds += unit switch
                {
                    "w" => value * 7L * 24L * 60L * 60L,
                    "d" => value * 24L * 60L * 60L,
                    "h" => value * 60L * 60L,
                    "m" => value * 60L,
                    "s" => value,
                    _ => 0
                };
            }
            if (totalSeconds > TimeSpan.MaxValue.TotalSeconds || totalSeconds < TimeSpan.MinValue.TotalSeconds)
                return false;

            result = TimeSpan.FromSeconds(totalSeconds);
            return true;
        }
        
        public static TimeSpan Parse(string input)
        {
            if (TryParse(input, out TimeSpan result))
            {
                return result;
            }
        
            return TimeSpan.Zero; 
        }
    }
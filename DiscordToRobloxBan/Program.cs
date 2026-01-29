

using CrownSilliBot.Misc;
using DiscordToRobloxBan.Helpers;

namespace DiscordToRobloxBan
{
    public static class Main_
    {
        public static void Main(string[] args)
        {
            Printer.Print( Serializer.SerializeToString(RobloxHandler.GetUsersFromUsername("SimobiSirOP").GetAwaiter().GetResult()));
        }
    }
}

using DiscordToRobloxBan.Helpers;
using DiscordToRobloxBan.RobloxAPI;

namespace DiscordToRobloxBan
{
    public static class Main_
    {
        public static void Main(string[] args)
        {
            RobloxApiClient robloxApiClient = new RobloxApiClient(Master.mainConfig.RobloxApiToken);
            Printer.Print(robloxApiClient.GetUsersFromUsernames(new[] { "valier999" }).GetAwaiter().GetResult()[0].DisplayName!, null, true);
        }
    }
}
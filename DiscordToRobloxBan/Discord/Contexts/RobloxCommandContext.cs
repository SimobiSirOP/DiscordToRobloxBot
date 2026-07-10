using NetCord;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;
using RobloxCloudApi;

namespace DiscordToRobloxBan.Discord.Contexts;

public class RobloxCommandContext : ApplicationCommandContext
{
    public RobloxCommandContext(ApplicationCommandInteraction interaction, RobloxApiClient robloxApiClient, GatewayClient client) : base(interaction, client)
    {
        RobloxApiClient = robloxApiClient;
    }

    public RobloxApiClient RobloxApiClient { get; private set; }
}
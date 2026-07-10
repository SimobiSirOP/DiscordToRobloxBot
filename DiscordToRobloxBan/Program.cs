
using System.Linq;
using DiscordToRobloxBan.Discord.Contexts;
using DiscordToRobloxBan.Discord.Modules;
using DiscordToRobloxBan.Helpers;
using NetCord;
using NetCord.Gateway;
using NetCord.Logging;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using RobloxCloudApi;

namespace DiscordToRobloxBan
{
    public static class Main_
    {
        private static GatewayClient Client { get; set; }

        public static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;


            var client = new GatewayClient(new BotToken(Master.MainConfig.DiscordBotToken),
                new GatewayClientConfiguration
                {
                    Logger = new PrinterLogger()
                });
            Client = client;
            

            var service = new ApplicationCommandService<RobloxCommandContext>();
            

            service.AddModule(typeof(AdminChangeCommand));
            service.AddModule(typeof(RobloxControlCommands));
            service.AddModule(typeof(RobloxDatastoreCommands));

            client.InteractionCreate += async interaction =>
            {
                if (interaction is not ApplicationCommandInteraction applicationCommandInteraction)
                    return;


                var result = await service.ExecuteAsync(new RobloxCommandContext(applicationCommandInteraction,
                    Master.RobloxClient, client));

                Printer.Print(
                    $"{applicationCommandInteraction.User.Id} executed command {applicationCommandInteraction.Data.Name}");

                if (result is IFailResult failResult)
                {
                    Printer.Print(failResult.Message, LogLevel.Error);
                    if (failResult is PreconditionFailResult)
                    {
                        await interaction.SendResponseAsync(InteractionCallback.Message(new()
                        {
                            Content = failResult.Message,
                            Flags = MessageFlags.Ephemeral | MessageFlags.SuppressEmbeds
                        }));
                        return;
                    }

                    await interaction.ModifyResponseAsync((options =>
                    {
                        options.Flags = MessageFlags.Ephemeral | MessageFlags.SuppressNotifications |
                                        MessageFlags.SuppressEmbeds;
                        options.Content = "Error has occured during execution of this command";
                    }));
                }

                return;
            };

            await service.RegisterCommandsAsync(client.Rest, client.Id);

            await client.StartAsync();
            await Task.Delay(-1);
        }

        private static async void CurrentDomainOnProcessExit(object? sender, EventArgs e)
        {
            await Client.CloseAsync();
        }

    }

}
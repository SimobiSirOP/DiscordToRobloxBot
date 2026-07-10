using DiscordToRobloxBan.Discord.Contexts;
using System.ComponentModel;
using DiscordToRobloxBan.Discord.Precondition;
using DiscordToRobloxBan.Files.ConfigFiles;
using DiscordToRobloxBan.Helpers;
using NetCord;
using NetCord.Logging;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using RobloxCloudApi;
using RobloxCloudApi.APITypes;

namespace DiscordToRobloxBan.Discord.Modules;

public class RobloxControlCommands : ApplicationCommandModule<RobloxCommandContext>
{
    [SlashCommand("robloxban", "Ban a roblox user.",
        Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel],
        IntegrationTypes = [ApplicationIntegrationType.GuildInstall, ApplicationIntegrationType.UserInstall])]
    [AdminRoleOnlyPrecondition<RobloxCommandContext>]
    public async Task BanRobloxUser(
        [SlashCommandParameter(Name = "username", Description = "The username of the roblox user.")]
        string username,
        [SlashCommandParameter(Name = "duration", Description = "Duration of a ban in seconds. Do not specify for permanent ban")]
        long? duration = null,
        [SlashCommandParameter(Name = "reason", Description = "Reason of a ban")]
        string reason = "No reason specified")
    {
        await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
        var message =  DiscordMessages.CreateMessage<InteractionMessageProperties>();

        if (duration <= -31557600000 || duration >= 31557600000)
        {
            message.Flags = MessageFlags.Ephemeral;
            message.Content = "Duration must be between 1 and 31557600000 seconds.";
            await Context.Interaction.ModifyResponseAsync((options => options.Content = message.Content));
            return;
        }
        
        RobloxUser user = await Context.RobloxApiClient.GetUserFromUsername(username)!;
        if (user == null)
        {
            message.Flags = MessageFlags.Ephemeral;
            message.Content = "User not found.";
            await Context.Interaction.ModifyResponseAsync((options => options.Content = message.Content));
            return;
        }

        var thumbnailTask = Context.RobloxApiClient.GenerateUserThumbnail(user.UserId, RobloxThumbnailSize.Size150, RobloxThumbnailFormat.PNG, RobloxThumbnailShape.SQUARE);
        var banTask = Context.RobloxApiClient.BanUserFromUniverse(user.UserId, Master.MainConfig.RobloxUniverseId.GetValueOrDefault(), duration, reason, reason);
        
        await Task.WhenAll(thumbnailTask, banTask);
        var thumbnailUrl = thumbnailTask.Result.Result!.PropData["imageUri"].ToString();
        var banResult = banTask.Result.GameJoinRestriction;
        
        var banDurationString = duration == null ? "Permanent" : 
            $"<t:{((DateTimeOffset)(banResult.StartTime.Value + (DurationParser.Parse(banResult.Duration))).ToUniversalTime())
            .ToUnixTimeSeconds()}:f>";
        
        EmbedProperties _embedProperties = new EmbedProperties()
            .WithTitle($"Banned user {user.Username}")
            .WithThumbnail(thumbnailUrl)
            .WithColor(new Color(255, 0, 0))
            .AddFields(
                new EmbedFieldProperties()
                    .WithName("Username")
                    .WithValue($"{user.Username} ({user.UserId.ToString()})"),
                new EmbedFieldProperties()
                    .WithName("Reason")
                    .WithValue(banResult?.DisplayReason),
                new EmbedFieldProperties()
                    .WithName("Until")
                    .WithValue(banDurationString));
        message.AddEmbeds(_embedProperties);
        
        
        Printer.Print($"Banned user {user.Username} ({user.UserId.ToString()})", LogLevel.Information, true);
        await Context.Interaction.ModifyResponseAsync((options => options.Embeds = message.Embeds));
    }

    [SlashCommand("robloxthumbnail", "Generate a thumbnail for the roblox user.",
        Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel],
        IntegrationTypes = [ApplicationIntegrationType.UserInstall, ApplicationIntegrationType.GuildInstall])]
    public async Task GetRobloxThumbnail(
        [SlashCommandParameter(Name = "username", Description = "The username of the roblox user.")] string username,
        [SlashCommandParameter(Name = "shape", Description = "Shape of the thumbnail.")] RobloxThumbnailShape thumbnailShape = RobloxThumbnailShape.ROUND)
    {
        await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
        
        var message =  DiscordMessages.CreateMessage<InteractionMessageProperties>();
        
        RobloxUser robloxUser = await Context.RobloxApiClient.GetUserFromUsername(username)!;
        if (robloxUser == null)
        {
            message.Flags = MessageFlags.Ephemeral;
            message.Content = "User not found.";
            await Context.Interaction.ModifyResponseAsync((options => options.Content = message.Content));
            return;
        }
            
        var thumbnailData = await Context.RobloxApiClient.GenerateUserThumbnail(robloxUser.UserId, RobloxThumbnailSize.Size420, RobloxThumbnailFormat.PNG, thumbnailShape);
        message.Content = thumbnailData.Result.PropData["imageUri"].ToString()!;
        
        await Context.Interaction.ModifyResponseAsync((options => options.Content = message.Content));
    }

    #region Roblox User Info getter
    
    private EmbedProperties GetEmbedFromRobloxUser(RobloxFullUser user)
    {
        var thumbnail = Master.RobloxClient.GenerateUserThumbnail(user.UserId, RobloxThumbnailSize.Size150,
            RobloxThumbnailFormat.PNG, RobloxThumbnailShape.SQUARE).GetAwaiter().GetResult();
        var thumbnailUrl = thumbnail.Result.PropData["imageUri"].ToString();

        var color = new Color(0, 0, 0);
        if (user.CreationTime.Value > DateTime.Now.AddYears(-1))
            color = new Color(255, 255, 0);
        else if (user.CreationTime.Value > DateTime.Now.AddMonths(-1))
            color = new Color(255, 0, 0);
        else
            color = new Color(0, 255, 0);
        
        EmbedProperties _embedProperties = new EmbedProperties()
            .WithTitle($"{user.DisplayName} ({user.Username})")
            .WithThumbnail(thumbnailUrl)
            .WithColor(color)
            .AddFields(
                new EmbedFieldProperties()
                    .WithName("Username")
                    .WithValue(user.Username),
                new EmbedFieldProperties()
                    .WithName("UserId")
                    .WithValue(user.UserId.ToString()),
                new EmbedFieldProperties()
                    .WithName("Account creation date")
                    .WithValue(user.CreationTime?.ToString("yyyy-MM-dd HH:mm:ss")));
        
        return _embedProperties;
    }
    
    
    [SlashCommand("robloxuserinfo", "Get roblox user info", 
        Contexts = [InteractionContextType.Guild, InteractionContextType.DMChannel], 
        IntegrationTypes = [ApplicationIntegrationType.UserInstall, ApplicationIntegrationType.GuildInstall])]

    public async Task GetRobloxUserInfo(
        [SlashCommandParameter] string robloxUsername)
    {
        await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

        var message =  DiscordMessages.CreateMessage<InteractionMessageProperties>();
        
        var robloxUser = await Context.RobloxApiClient.GetUserFromUsername(robloxUsername)!;
        if (robloxUser == null)
        {
            message.Content = "User not found.";
            await Context.Interaction.ModifyResponseAsync((options => options.Content = message.Content));
            return;
        }
        
        var robloxFullUser = await Context.RobloxApiClient.GetFullUserFromId(robloxUser.UserId);
        
        message.AddEmbeds(GetEmbedFromRobloxUser(robloxFullUser));
        await Context.Interaction.ModifyResponseAsync((options => options.Embeds = message.Embeds));
    }
    
    #endregion
    
}
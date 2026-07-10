using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using DiscordToRobloxBan.Files;
using DiscordToRobloxBan.Files.ConfigFiles;
using DiscordToRobloxBan.Helpers;
using NetCord;
using NetCord.Gateway;
using NetCord.Logging;
using RobloxCloudApi;
using LogLevel = NetCord.Logging.LogLevel;

namespace DiscordToRobloxBan;

public static class Master
{
    public static BotSettings botSettings { get; } = new BotSettings();
    public static AdminFile AdminRolesFile { get; set; } = new AdminFile(
        Path.Combine(botSettings.ApplicationDataPath, "adminRoles.json"));

    public static MainConfig MainConfig { get; private set; } =
        new MainConfig(Path.Combine(botSettings.ConfigsPath, "config.json"));
    
    public static RobloxApiClient RobloxClient { get; private set; } 
    
    static Master()
    {
        bool isConfigExists = File.Exists(Path.Combine(botSettings.ConfigsPath, "config.json"));
        if (!Directory.Exists(botSettings.ConfigsPath))
            Directory.CreateDirectory(botSettings.ConfigsPath);
        if (!Directory.Exists(botSettings.LogsPath))
            Directory.CreateDirectory(botSettings.LogsPath);
        if (!Directory.Exists(botSettings.ApplicationDataPath))
            Directory.CreateDirectory(botSettings.ApplicationDataPath);


        MainConfig = MainConfig.Load();
        AdminRolesFile = AdminRolesFile.Load();
        
        if (!isConfigExists)
        {
            Printer.Print(
                "Config file wasn't specified. It was created in directory of this application in Configs folder.",
                LogLevel.Critical, true);
            Environment.Exit(1);
        }
        try
        {
            //ValidateApiKeys().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Printer.Print("Error validating correct API keys: " + ex.Message, 
                LogLevel.Critical, true );
            Environment.Exit(1);
        }

        RobloxClient = new RobloxApiClient(new RobloxApiClientSettings(Master.MainConfig.RobloxApiToken));
    }
}
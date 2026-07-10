using System.Text.Json.Serialization;
using DiscordToRobloxBan.Helpers;
using NetCord.Logging;

namespace DiscordToRobloxBan.Files.ConfigFiles;

public  class MainConfig : ConfigFileBase<MainConfig>
{
    public string RobloxApiToken = "";
    
    public string DiscordBotToken = "";

    public long? RobloxUniverseId = 0;
    
    public ulong MainGuildId = 0;
    
    public LogLevel LogLevel = LogLevel.Information;
    

    [JsonIgnore] public sealed override string PathToFile { get; protected set; } = null!;

    public MainConfig() {}

    public MainConfig(string pathToFile)
    {
        PathToFile = pathToFile;
    }
}
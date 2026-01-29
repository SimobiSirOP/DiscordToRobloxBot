using System.Text.Json.Serialization;
using DiscordToRobloxBan.Helpers;

namespace DiscordToRobloxBan.Files;

public  class MainConfig : ConfigFile<MainConfig>
{
    public string RobloxApiToken = "";
    
    public string DiscordBotToken = "";

    public long RobloxPlaceId = 0;

    public string BanMessage = "Use formatted string, arguments: {name}";

    public LogImportance LogImportance = LogImportance.Notice;

    [JsonIgnore] public override string PathToFile { get; } = Path.Combine(Master.ConfigsPath, "config.json");
    
}
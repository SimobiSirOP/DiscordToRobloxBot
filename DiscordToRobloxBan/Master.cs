using DiscordToRobloxBan.Files;
using DiscordToRobloxBan.Helpers;

namespace DiscordToRobloxBan;

public static class Master
{
    public static string MasterPath = Directory.GetCurrentDirectory();

    public static string ConfigsPath = Path.Combine(MasterPath, "Configs");

    public static string LogsPath = Path.Combine(MasterPath, "Logs");

    public static MainConfig mainConfig = new MainConfig();
    

    static Master()
    {
        bool isConfigExists = File.Exists(Path.Combine(ConfigsPath, "config.json"));
        if (!Directory.Exists(ConfigsPath))
            Directory.CreateDirectory(ConfigsPath);
        if (!Directory.Exists(LogsPath))
            Directory.CreateDirectory(LogsPath);
        
        
        mainConfig = mainConfig.Load();
        if (!isConfigExists)
        {
            Printer.Print("Config file wasn't specified. It was created in directory of this application in Configs folder.", LogImportance.Alert, true);
            Environment.Exit(1);
        }
    }
}
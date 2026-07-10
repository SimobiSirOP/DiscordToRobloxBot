namespace DiscordToRobloxBan;

public class BotSettings
{
    public string MasterPath = Directory.GetCurrentDirectory();

    public string ConfigsPath  => Path.Combine(MasterPath, "Configs");

    public string LogsPath => Path.Combine(MasterPath, "Logs");

    public string ApplicationDataPath => Path.Combine(MasterPath, "Data");

}
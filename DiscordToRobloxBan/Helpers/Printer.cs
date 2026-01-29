using System.Text;

namespace DiscordToRobloxBan.Helpers;

public static class Printer
{
    public static void Print(string message, LogImportance importance = LogImportance.Info, bool alwaysWrite = false)
    {
        if (Master.mainConfig.LogImportance >= importance || alwaysWrite)
        {
            Console.WriteLine(message);
            WriteToLogs(message, importance);
        }
    }

    public static void WriteToLogs(string message, LogImportance importance = LogImportance.Info)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"[{DateTime.Now:HH:mm:ss}] | ");
        if (importance <= LogImportance.Warning)
            stringBuilder.Append($"[{nameof(importance)}] ");
        stringBuilder.Append($"{message}");
        stringBuilder.Append(Environment.NewLine);

        var dateTime = DateTime.Now.Date;

        var fileName = $"{dateTime.Year}-{dateTime.Month.ToString("D2")}-{dateTime.Day.ToString("D2")}";
        var fullPath = Master.LogsPath + Path.DirectorySeparatorChar + fileName + ".txt";

        File.AppendAllText(fullPath, stringBuilder.ToString());
        stringBuilder.Clear();
    }
}

public enum LogImportance
{
    Alert = 1,
    Critical,
    Error,
    Warning,
    Notice,
    Info,
    Debug
}
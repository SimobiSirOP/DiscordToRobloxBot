using System.Text;
using NetCord.Logging;

namespace DiscordToRobloxBan.Helpers;

public static class Printer
{
    public static void Print(string message, LogLevel importance = LogLevel.Information, bool alwaysWrite = false)
    {
        if (Master.MainConfig.LogLevel <= importance || alwaysWrite)
        {
            Console.WriteLine(message);
            WriteToLogs(message, importance);
        }
    }

    public static void WriteToLogs(string message, LogLevel importance = LogLevel.Information)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"[{DateTime.Now:HH:mm:ss}] | ");
        if (importance <= LogLevel.Warning)
            stringBuilder.Append($"[{importance.ToString().ToUpper()}] ");
        stringBuilder.Append($"{message}");
        stringBuilder.Append(Environment.NewLine);

        var dateTime = DateTime.Now.Date;

        var fileName = $"{dateTime.Year}-{dateTime.Month.ToString("D2")}-{dateTime.Day.ToString("D2")}";
        var fullPath = Master.botSettings.LogsPath + Path.DirectorySeparatorChar + fileName + ".txt";

        File.AppendAllText(fullPath, stringBuilder.ToString());
        stringBuilder.Clear();
    }
    
}

public class PrinterLogger(LogLevel minimumLogLevel = LogLevel.Information) : IGatewayLogger, IRestLogger, IVoiceLogger
{
    bool IsLogLevelEnabled(LogLevel logLevel, LogLevel minLogLevel)
    {
        return  logLevel >= minLogLevel;
    }
    void IGatewayLogger.Log<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        {
            if (!IsLogLevelEnabled(logLevel, minimumLogLevel))
                return;
            
            Printer.Print(formatter(state, exception), logLevel);
        }
    }

    bool IVoiceLogger.IsEnabled(LogLevel logLevel)
    {
        return IsLogLevelEnabled(logLevel, minimumLogLevel);
    }

    void IVoiceLogger.Log<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        {
            if (!IsLogLevelEnabled(logLevel, minimumLogLevel))
                return;
            
            Printer.Print(formatter(state, exception), logLevel);
        }
    }

    bool IRestLogger.IsEnabled(LogLevel logLevel)
    {
        return IsLogLevelEnabled(logLevel, minimumLogLevel);
    }

    void IRestLogger.Log<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        {
            if (!IsLogLevelEnabled(logLevel, minimumLogLevel))
                return;
            
            Printer.Print(formatter(state, exception), logLevel);
        }
    }

    bool IGatewayLogger.IsEnabled(LogLevel logLevel)
    {
        return IsLogLevelEnabled(logLevel, minimumLogLevel);
    }
}

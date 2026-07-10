using System.Text.Json.Serialization;
using DiscordToRobloxBan.Helpers;

namespace DiscordToRobloxBan.Files.ConfigFiles;


public abstract class ConfigFileBase<T> where T : ConfigFileBase<T>, new()
{
    [JsonIgnore] public abstract string PathToFile { get; protected set; }

    public T Load()
    {
        if (!File.Exists(PathToFile))
        {
            var file = new T();
            Serializer.SerializeToFile(PathToFile, this);
            return file;
        }

        var result = Serializer.SerializeFromFile<T>(Path.Combine(PathToFile));
        Serializer.SerializeToFile(Path.Combine(PathToFile), result);
        result.PathToFile = PathToFile;
        return result;
    }

    public void Save()
    {
        Serializer.SerializeToFile(PathToFile, this);
    }
}
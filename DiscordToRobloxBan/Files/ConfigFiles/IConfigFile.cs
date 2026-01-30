using System.Text.Json.Serialization;
using DiscordToRobloxBan.Helpers;

namespace DiscordToRobloxBan.Files;


public abstract class ConfigFile<T> where T : ConfigFile<T>, new()
{
    [JsonIgnore] public abstract string PathToFile { get; }

    public virtual T Load()
    {
        if (!File.Exists(PathToFile))
        {
            var file = new T();
            Serializer.SerializeToFile(PathToFile, this);
            return file;
        }

        var result = Serializer.SerializeFromFile<T>(Path.Combine(PathToFile));
        Serializer.SerializeToFile(Path.Combine(PathToFile), result);
        return result;
    }

    public virtual void Save<T>(T file)
    {
        Serializer.SerializeToFile(PathToFile, file);
    }
}
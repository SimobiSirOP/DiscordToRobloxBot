using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;

namespace CrownSilliBot.Misc;

public class Serializer
{
    private static JsonSerializerOptions DefaultSettings => new()
    {
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
        AllowTrailingCommas = true,
        ReadCommentHandling =  JsonCommentHandling.Skip,
    };


    private static JsonSerializerOptions IndentedSettings => new(DefaultSettings)
    {
        WriteIndented = true
    };

    /// <summary>
    ///     Serializes an object to JSON/>
    /// </summary>
    public static string SerializeToString(object serializable)
    {
        return JsonSerializer.Serialize(serializable, DefaultSettings);
    }


    /// <summary>
    ///     Deserializes an object from JSON/>
    /// </summary>
    public static T SerializeFromString<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, DefaultSettings);
    }


    /// <summary>
    ///     Serializes an object to JSON file/>
    /// </summary>
    public static void SerializeToFile(string path, object serializable)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(serializable, IndentedSettings));
    }

    /// <summary>
    ///     Deserializes an object from JSON file/>
    /// </summary>
    public static T SerializeFromFile<T>(string path)
    {
        return JsonSerializer.Deserialize<T>(File.ReadAllText(path), DefaultSettings);
    }

    public static bool CheckFileSerializability<T>(string path)
    {
        try
        {
            SerializeFromFile<T>(path);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool CheckStringSerializability<T>(string str)
    {
        try
        {
            SerializeFromString<T>(str);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
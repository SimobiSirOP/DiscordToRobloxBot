using System.Text;
using System.Text.Json.Nodes;
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
            Printer.Print(
                "Config file wasn't specified. It was created in directory of this application in Configs folder.",
                LogImportance.Alert, true);
            Environment.Exit(1);
        }
        try
        {
            ValidateApiKeys().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Printer.Print("Error validating correct API keys: " + ex.Message, LogImportance.Alert, true );
            Environment.Exit(1);
        }
    }

    private static async Task ValidateApiKeys()
    {
        await ValidateRobloxApiKey();
    }
    private static async Task ValidateRobloxApiKey()
    {
        HttpClientHandler httpClientHandler = new HttpClientHandler();
        HttpClient validationHttpClient = new HttpClient(httpClientHandler);

        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://apis.roblox.com/api-keys/v1/introspect"))
        {
            // Getting API Info
            request.Content =
                new StringContent(Serializer.SerializeToString(new { apiKey = mainConfig.RobloxApiToken }), new ASCIIEncoding(), "application/json");
            using HttpResponseMessage response = await validationHttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                JsonNode? errorJson = Serializer.SerializeFromString<JsonNode>(await response.Content.ReadAsStringAsync());
                if (errorJson["message"] != null)
                    throw new Exception(errorJson["message"]?.ToString());
                else
                    throw new Exception("Error getting messages, sending response instead:" + await response.Content.ReadAsStringAsync());
            }
            
            // Validating if has atleast some parameters
            JsonNode? resultJson = Serializer.SerializeFromString<JsonNode>(await response.Content.ReadAsStringAsync());
            if (resultJson["scopes"] == null)
                throw new Exception("No scopes of API detected. Please provide permissions to API token");
            
            JsonNode? scopes = resultJson["scopes"];
            JsonNode? permissionScope = null;
            permissionScope = scopes!.AsArray().FirstOrDefault(x => x!["name"] != null && x["name"]!.ToString() == "universe.user-restriction");
            
            // Validating if has a required permission
            if (permissionScope == null 
                || permissionScope["operations"] == null)
                throw new Exception("Bot requires universe.user-restriction set with \"write\" and \"read\" available operations in Roblox API token.");
            
            // Validating if has permission flags
            var availableOperations  = permissionScope["operations"]!.AsArray().GetValues<string>();
            if (!availableOperations.Contains("read")
                || !availableOperations.Contains("write"))
                throw new Exception("Bot requires universe.user-restriction set with \"write\" and \"read\" available operations in Roblox API token.");

            
            // Validating universe permissions
            if (permissionScope["universeIds"] == null)
                throw new Exception("Haven't found universes parameter. Shouldn't happen, validate your roblox API Token.");
            var availableUniverses = permissionScope["universeIds"].AsArray().GetValues<string>();
            
            if (availableUniverses.Contains("*") && !availableUniverses.Contains(mainConfig.RobloxUniverseId.ToString()))
                throw new Exception("Roblox API token is configured for another universe.");
        }
    }
}
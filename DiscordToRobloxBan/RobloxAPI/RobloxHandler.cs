/*using System.Globalization;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using DiscordToRobloxBan.Files;
using DiscordToRobloxBan.Helpers;

namespace DiscordToRobloxBan;

public static class RobloxHandler
{
    private static HttpClientHandler RobloxHttpClientHandler = new HttpClientHandler()
    {
        SslProtocols = SslProtocols.Tls13
    };
    public static HttpClient RobloxHttpClient = new HttpClient(RobloxHttpClientHandler)
    {
        BaseAddress = new Uri("https://roblox.com/api"),
        DefaultRequestHeaders = { {"x-api-key", Master.mainConfig.RobloxApiToken} }
    };
    
    private static readonly string UsersApiPath  = "https://users.roblox.com";
    
    private static readonly string MainApiPath  = "https://apis.roblox.com";


    public static async Task<RobloxUser[]> GetUsersFromUsernames(string[] usernames, bool excludeBannedUsers = true)
    {
        var requestData = new
        {
            usernames = usernames,
            excludeBannedUsers,
        };
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, UsersApiPath + "/v1/usernames/users" ) 
            {Content = new StringContent(Serializer.SerializeToString(requestData), Encoding.UTF8, "application/json")};
        using HttpResponseMessage usernameResponseMessage = RobloxHttpClient.SendAsync(request).Result;
        usernameResponseMessage.EnsureSuccessStatusCode();
        
        string usernamesResult = await usernameResponseMessage.Content.ReadAsStringAsync();
     
        return Serializer.SerializeFromString<RobloxUser[]>(usernamesResult, "data");
    }

    public static async Task<RobloxUser?> GetUserFromUsername(string username)
    {
        RobloxUser[] robloxUsers = await GetUsersFromUsernames([username]);
        if (robloxUsers.Length == 0)
            return null;
        
        foreach (RobloxUser robloxUser in robloxUsers)
            if (robloxUser.Username == username) return robloxUser;
        
        return null;
    }
    
    public static async Task BanUserFromRoblox(RobloxUser user, long duration, string reason = "You have been banned", bool includeAlts = true)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        using HttpRequestMessage
            banRequest = new HttpRequestMessage(HttpMethod.Patch, MainApiPath + $@"/cloud/v2/universes/{Master.mainConfig.RobloxUniverseId}/user-restrictions/{user.UserId}");
        var gameJoinRestrictionData = new Dictionary<string, object>()
        {
            ["active"] = true,
            ["startTime"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
            
        ["privateReason"] = reason,
        [   "displayReason"] = reason,
        [   "excludeAltAccounts"] = includeAlts,
        [   "inherited"] = true
        };
        if (duration == -1)
            gameJoinRestrictionData.Add("duration", duration);
        var responseContent = new
        {
            path = $"universes/{Master.mainConfig.RobloxUniverseId}/user-restrictions/{user.UserId}",
            gameJoinRestriction = gameJoinRestrictionData
        };
        
        banRequest.Content = new StringContent(Serializer.SerializeToString(responseContent), Encoding.UTF8, "application/json");
        var check = Serializer.SerializeToString(responseContent);
        using HttpResponseMessage banResponseMessage = await RobloxHttpClient.SendAsync(banRequest);
        if (!banResponseMessage.IsSuccessStatusCode)
            throw new TimeoutException(banResponseMessage.ReasonPhrase);
    }
    
    public static async Task BanUserFromRoblox(string username, long duration, string reason = "You have been banned", bool includeAlts = true)
    {
        RobloxUser? user = await GetUserFromUsername(username);
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        await BanUserFromRoblox(user, duration, reason, includeAlts);
    }
}
*/
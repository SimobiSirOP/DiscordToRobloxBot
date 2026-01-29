using System.Security.Authentication;
using System.Text.Json.Nodes;
using CrownSilliBot.Misc;
using DiscordToRobloxBan.Files;

namespace DiscordToRobloxBan;

public static class RobloxHandler
{
    private static HttpClientHandler RobloxHttpClientHandler = new HttpClientHandler()
    {
        SslProtocols = SslProtocols.Tls13
    };
    public static HttpClient RobloxHttpClient = new HttpClient(RobloxHttpClientHandler)
    {
        BaseAddress = new Uri("https://roblox.com/api/v1/"),
        DefaultRequestHeaders = { {"x-api-key", Master.mainConfig.RobloxApiToken} }
    };


    public static async Task<RobloxUser[]> GetUsersFromUsername(string username, bool excludeBannedUsers = true)
    {
        var requestData = new
        {
            usernames = new[] { username },
            excludeBannedUsers,
        };
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Serializer.SerializeToString(requestData));
        HttpResponseMessage usernameResponseMessage = RobloxHttpClient.SendAsync(request).Result;
        return Serializer.SerializeFromString<RobloxUser[]>(await usernameResponseMessage.Content.ReadAsStringAsync());
    }
}
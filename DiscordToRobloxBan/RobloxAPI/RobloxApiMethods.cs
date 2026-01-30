using System.Runtime.CompilerServices;
using DiscordToRobloxBan.RobloxAPI.Helpers;
using DiscordToRobloxBan.RobloxAPI.Requests;
using DiscordToRobloxBan.RobloxAPI.Types;
using NetCord;

namespace DiscordToRobloxBan.RobloxAPI;

public static class RobloxApiMethods
{
        public static async Task<RobloxUser[]> GetUsersFromUsernames(
                this RobloxApiClient client, 
                string[ ] usernames,
                bool excludeBannedUsers = true
        )
        {
                return (await client.ThrowIfNull().SendRequest(new GetUsersFromUsernamesRequest()
                {
                        Usernames = usernames,
                        ExcludeBannedUsers = excludeBannedUsers
                }))!;
        }
        
        public static async Task<RobloxUser> GetUserFromUsername(
                this RobloxApiClient client,
                string username)
        {
                RobloxUser[] robloxUsers = await client.GetUsersFromUsernames([username]);
                if (robloxUsers.Length == 0) 
                        return null;
        
                foreach (RobloxUser robloxUser in robloxUsers)
                        if (robloxUser.Username == username) return robloxUser;
        
                return null;
        }
}
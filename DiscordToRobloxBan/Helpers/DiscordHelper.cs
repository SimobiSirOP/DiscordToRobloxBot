using NetCord.Rest;

namespace DiscordToRobloxBan.Helpers;

public static  class DiscordMessages
{
    public static T CreateMessage<T>() where T : IMessageProperties, new()
    {
        T message = new();

        message
            .WithComponents([]);

        return message;
    } 
}
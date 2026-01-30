using System.Text;
using System.Text.Json.Serialization;
using DiscordToRobloxBan.Helpers;

namespace DiscordToRobloxBan.RobloxAPI.Requests;

public abstract class RequestBase<TResponse> : IRequest<TResponse>
{
    [JsonIgnore] 
    public abstract HttpMethod HttpMethod { get; }
    
    [JsonIgnore]
    public abstract string RequestPath { get; }

    [JsonIgnore] public virtual string DeserializedPropertyPath { get; } = "";
    
    public HttpContent? GetHttpContent()
    {
        return new StringContent(Serializer.SerializeToString(this), Encoding.UTF8, "application/json");
    }
}
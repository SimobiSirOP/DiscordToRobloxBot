using System.Text.Json.Serialization;

namespace DiscordToRobloxBan.RobloxAPI.Requests;

public interface IRequest
{
    [JsonIgnore]
    public HttpMethod HttpMethod { get; }
    
    [JsonIgnore]
    public string RequestPath { get; }
    
    [JsonIgnore]
    public string DeserializedPropertyPath { get; }

    public HttpContent? GetHttpContent();
}

public interface IRequest<TResponse> : IRequest;
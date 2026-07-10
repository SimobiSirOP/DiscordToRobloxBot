using System.Text.Json.Serialization;
using NetCord;

namespace DiscordToRobloxBan.Files.ConfigFiles;

public class AdminFile : ConfigFileBase<AdminFile>
{
    public ulong MainOwner { get; set; }
    
    public List<ulong> AdminRolesIds {get; set; } = new List<ulong>();

    public List<ulong> AdminUserIds { get; set; } = new List<ulong>();

    [JsonIgnore] public sealed override string PathToFile { get; protected set; } = null!;
    
    public AdminFile() {}

    public AdminFile(string pathToFile)
    {
        PathToFile = pathToFile;
    }
    
}
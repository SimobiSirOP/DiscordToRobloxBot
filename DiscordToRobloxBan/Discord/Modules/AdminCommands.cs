
using System.Text;
using DiscordToRobloxBan.Discord.Contexts;
using DiscordToRobloxBan.Discord.Precondition;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace DiscordToRobloxBan.Discord.Modules;

[SlashCommand("admins", "Change admins", 
    Contexts = [InteractionContextType.Guild, InteractionContextType.BotDMChannel], 
    IntegrationTypes = [ApplicationIntegrationType.GuildInstall])]
[OwnerOnly<RobloxCommandContext>]
public class AdminChangeCommand() : ApplicationCommandModule<RobloxCommandContext>
{
    [SubSlashCommand("roleadd", "Add a admin role")]
    
    public string AddAdminRole(
        [SlashCommandParameter(Name = "role", Description = "The role to add to the admin roles")] Role role)
    {
        if (Master.AdminRolesFile.AdminRolesIds.Contains(role.Id))
            return  $"<@&{role.Id}> is already admin role.";
        
        Master.AdminRolesFile.AdminRolesIds.Add(role.Id);
        Master.AdminRolesFile.Save();
        return $"Added <@&{role.Id}> to admin roles.";
    }
    
    [SubSlashCommand("roleremove", "Remove a admin role")]
    public string RemoveAdminRole(
        [SlashCommandParameter(Name = "role", Description = "The role to remove from admin roles" )] Role role)
    {
        if (!Master.AdminRolesFile.AdminRolesIds.Contains(role.Id))
            return  $"You can't remove <@&{role.Id}> from admin roles because it isn't a admin role.";
        
        Master.AdminRolesFile.AdminRolesIds.Remove(role.Id);
        Master.AdminRolesFile.Save();
        return $"Removed <@&{role.Id}> from admin roles.";
    }

    [SubSlashCommand("add", "Add new hard admin (Use roles if possible)")]
    public string AddAdmin(
        [SlashCommandParameter(Name = "user", Description = "The user to add to admin list")] User user)
    {
        if (Master.AdminRolesFile.AdminUserIds.Contains(user.Id))
            return  $"You can't add <@{user.Id}> to hard admins because this user is already a admin.";
        
        Master.AdminRolesFile.AdminUserIds.Add(user.Id);
        Master.AdminRolesFile.Save();
        return $"Added <@{user.Id}> to admins.";
    }
    
    [SubSlashCommand("remove", "Remove hard admin (Use roles if possible)")]
    public string RemoveAdmin(
        [SlashCommandParameter(Name = "user", Description = "The user to remove from admin list")] User user)
    {
        if (!Master.AdminRolesFile.AdminUserIds.Contains(user.Id))
            return  $"You can't remove <@{user.Id}> from hard admins because this user isn't a admin.";
        
        Master.AdminRolesFile.AdminUserIds.Remove(user.Id);
        Master.AdminRolesFile.Save();
        return $"Removed <@{user.Id}> from admins.";
    }
    
    [SubSlashCommand("get", "Retrieve all admins")]
    public InteractionMessageProperties GetAdminRoles()
    {
        StringBuilder stringBuilder = new StringBuilder();
        var adminRolesCount = Master.AdminRolesFile.AdminRolesIds.Count;
        var adminUsersCount = Master.AdminRolesFile.AdminUserIds.Count;
        
        if (adminRolesCount == 0 && adminUsersCount == 0)
            return "There are no admins.";
        
        if (adminRolesCount > 0)
        {
            stringBuilder.AppendLine("A list of roles marked as Admin roles:\n");
            foreach (var adminRoleId in Master.AdminRolesFile.AdminRolesIds)
            {
                stringBuilder.AppendLine($"* <@&{adminRoleId}>");
            }
        }

        if (adminUsersCount > 0)
        {
            stringBuilder.AppendLine("A list of hardcoded admins:\n");
            foreach (var adminUserId in Master.AdminRolesFile.AdminUserIds)
            {
                stringBuilder.AppendLine($"* <@{adminUserId}>");
            }
        }
        InteractionMessageProperties message = stringBuilder.ToString();
        message.AllowedMentions = AllowedMentionsProperties.None;

        return message;
    }
}

using NetCord;
using NetCord.Services;

namespace DiscordToRobloxBan.Discord.Precondition;

public class AdminRoleOnlyPrecondition<TContext> : PreconditionAttribute<TContext> where TContext : IGuildContext, IUserContext
{
    public override ValueTask<PreconditionResult> EnsureCanExecuteAsync(TContext context, IServiceProvider? serviceProvider)
    {
        if (context.User.Id == Master.AdminRolesFile.MainOwner)
            return new(PreconditionResult.Success);
        
        
        if (Master.AdminRolesFile.AdminUserIds.Contains(context.User.Id))
            return new(PreconditionResult.Success);

        var guild = context.Guild;
        if (guild is null)
            return new(PreconditionResult.Fail("The current guild could not be found."));
        
        if (context.User is GuildUser guildUser)
        {
            foreach (var role in Master.AdminRolesFile.AdminRolesIds)
            {
                if (guildUser.RoleIds.Contains(role))
                    return new(PreconditionResult.Success);
            }
        }
        
        return new(PreconditionResult.Fail("You are not an admin."));
    }
}
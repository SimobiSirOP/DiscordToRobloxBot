using NetCord.Services;

namespace DiscordToRobloxBan.Discord.Precondition;


    public class OwnerOnly<TContext> : PreconditionAttribute<TContext> where TContext : IUserContext
    {
        public override ValueTask<PreconditionResult> EnsureCanExecuteAsync(TContext context, IServiceProvider? serviceProvider)
        {
            if (context.User.Id == Master.AdminRolesFile.MainOwner)
                return new(PreconditionResult.Success);
            else
            {
                return new(PreconditionResult.Fail($"Only <@{Master.AdminRolesFile.MainOwner}> can execute this."));
            }
        }
    }

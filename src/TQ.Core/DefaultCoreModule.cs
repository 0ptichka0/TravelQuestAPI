using Autofac;
using TQ.Core.Interfaces;
using TQ.Core.Services;

namespace TQ.Core
{
    public class DefaultCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CPRunService>()
                .As<ICPRunService>().InstancePerLifetimeScope();

            builder.RegisterType<CPService>()
                .As<ICPService>().InstancePerLifetimeScope();

            builder.RegisterType<CPVisitService>()
                .As<ICPVisitService>().InstancePerLifetimeScope();

            builder.RegisterType<RunService>()
                .As<IRunService>().InstancePerLifetimeScope();

            builder.RegisterType<TeamResultService>()
                .As<ITeamResultService>().InstancePerLifetimeScope();

            builder.RegisterType<TeamService>()
                .As<ITeamService>().InstancePerLifetimeScope();

            builder.RegisterType<UserService>()
                .As<IUserService>().InstancePerLifetimeScope();
        }
    }
}

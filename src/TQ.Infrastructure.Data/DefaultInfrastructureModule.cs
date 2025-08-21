using Autofac;
using TQ.Infrastructure.Data.TravelQuestDb;
using TQ.SharedKernel;
using TQ.SharedKernel.Interfaces;

namespace TQ.Infrastructure.Data
{

    public class DefaultInfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder) {

            builder.RegisterGeneric(typeof(TravelQuestRepository<>))
               .As(typeof(ITravelQuestRepository<>))
               .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventDispatcher>()
               .As<IDomainEventDispatcher>()
               .InstancePerLifetimeScope();
        }
    }
}

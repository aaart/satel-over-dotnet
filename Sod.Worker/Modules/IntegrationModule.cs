using Autofac;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Satel.Communication;
using Sod.Infrastructure.Satel.State.Events;
using Sod.Infrastructure.Satel.State.Events.Mqtt;
using Sod.Infrastructure.Satel.State.Loop;

namespace Sod.Worker.Modules
{
    public class IntegrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Manipulator>().As<IManipulator>().SingleInstance();
            builder.RegisterType<StepCollectionFactory>().As<IStepCollectionFactory>().SingleInstance();
            builder.RegisterType<MqttOutgoingChangeNotifier>().As<IOutgoingChangeNotifier>().SingleInstance();
        }
    }
}
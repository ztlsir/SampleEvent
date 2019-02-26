using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SampleEvent.LightInject
{
    public class SampleEventIOCFactory
    {
        public static void RegisterEventHandlersFromAssemblies(IServiceContainer serviceContainer, Func<ILifetime> lifetimeFactory,
           params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                serviceContainer.RegisterAssembly(assembly, lifetimeFactory, (interfaceType, eventHandlerType) =>
                    interfaceType.IsConstructedGenericType &&
                    interfaceType.GetGenericTypeDefinition() == typeof(IEventHandler<>));
            }
        }
    }
}

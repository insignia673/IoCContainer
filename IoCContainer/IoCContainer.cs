using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer
{
    public class IoCContainer
    {
        private ContainerBuilder _containerBuilder;
        public IoCContainer()
        {
            _containerBuilder = new ContainerBuilder();
        }

        public void Register<TInterface, TType>() where TType : TInterface
        {
            _containerBuilder.Add(typeof(TInterface), typeof(TType));
        }

        public TType Resolve<TType>()
        {
            return (TType)_containerBuilder.GetInstance(typeof(TType));
        }

        public void RegisterAll()
        {
            var classes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(c => c.Namespace == "IoCContainer.Services");

            var interfaces = Assembly.GetExecutingAssembly().GetTypes()
                .Where(c => c.Namespace == "IoCContainer.Contracts");

            foreach (var c in classes)
            {
                var Ic = interfaces.First(i => i.Name == 'I' + c.Name);
                _containerBuilder.Add(Ic, c);
            }
        }
    }

    public class ContainerBuilder
    {
        private Dictionary<Type, Type> contractAndClass = new Dictionary<Type, Type>();

        public void Add(Type contract, Type instance)
        {
            contractAndClass.Add(contract, instance);
        }

        public object GetInstance(Type type)
        {
            if (!contractAndClass.TryGetValue(type, out var resultType))
            {
                throw new InvalidOperationException($"{type.Name} does not exist");
            }

            var instance = contractAndClass[type];

            var ctor = instance.GetConstructors()[0];
            var args = ctor.GetParameters()
                .Select(x => GetInstance(x.ParameterType))
                .ToArray();

            return Activator.CreateInstance(instance, args);
        }
    }
}

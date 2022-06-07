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

        public void Register<TInterface, TType>()
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
                var Ic = interfaces.FirstOrDefault(i => i.Name == 'I' + c.Name);
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
            var instance = contractAndClass[type];

            var ctor = instance.GetConstructors()[0];
            var param = ctor.GetParameters();

            var parameters = param
                .Where(x => contractAndClass.ContainsKey(x.ParameterType))
                .Select(x => x.ParameterType)
                .ToArray();

            object[] args = new object[parameters.Count()];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = GetInstance(parameters[i]);
            }

            return Activator.CreateInstance(instance, args);
        }
    }
}

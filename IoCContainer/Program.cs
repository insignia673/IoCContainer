using IoCContainer.Contracts;
using IoCContainer.Services;
using System;

namespace IoCContainer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = new IoCContainer();
            container.RegisterAll();
            //container.Register<IFoo, Foo>();
            //container.Register<IBar, Bar>();
            //container.Register<IBaz, Baz>();
            var foo = container.Resolve<IFoo>();
            foo.DoStuff();
        }
    }
}
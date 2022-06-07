using IoCContainer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.Services
{
    public class Foo : IFoo
    {
        private readonly IBar _bar;
        private readonly IBaz _baz;
        public Foo(IBar bar, IBaz baz)
        {
            _bar = bar;
            _baz = baz;
        }
        public void DoStuff()
        {
            _baz.Print(_bar.Message);
        }
    }
}

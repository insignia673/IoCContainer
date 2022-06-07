﻿using IoCContainer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.Services
{
    public class Bar : IBar
    {
        public string Message => "Hello, IoC!";
    }
}

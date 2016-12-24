﻿using SampleExtensibilityCore;
using System;

namespace SamplePlugin
{
    public class HelloWorldPlugin : BasePlugin, IConsolePlugin
    {
        public override void OnLoaded()
        {
            //Nothing to do here
            base.OnLoaded();
        }

        public void PrintSomething()
        {
            Console.WriteLine("Hello, world from the plugin!");
        }
    }
}
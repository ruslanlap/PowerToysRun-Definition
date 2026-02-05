// Copyright (c) ruslanlap
// Licensed under the MIT license.

using Microsoft.CommandPalette.Extensions;
using Shmuelie.WinRTServer;
using System;
using System.Threading;

namespace DefinitionExtension;

public class Program
{
    [MTAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "-RegisterProcessAsComServer")
        {
            using var server = new ComServer();
            var extensionDisposedEvent = new ManualResetEvent(false);
            var extensionInstance = new DefinitionExtension(extensionDisposedEvent);
            server.RegisterClass<DefinitionExtension>(() => extensionInstance);
            server.Start();
            extensionDisposedEvent.WaitOne();
        }
        else
        {
            Console.WriteLine("Not being launched as an Extension... exiting.");
        }
    }
}

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sod.Infrastructure;
using Sod.Infrastructure.Satel;
using Sod.Infrastructure.Satel.Socket;

namespace Sod.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cfg = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .Build();

            var address = cfg.GetValue<string>("Satel:Address");
            var port = cfg.GetValue<int>("Satel:Port");
            using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(address, port);
            var manipulator = new Manipulator(new SocketSender(socket), new SocketReceiver(socket), cfg.GetValue<string>("Satel:UserCode"));

            var outputsToSwtch = new bool[128];
            outputsToSwtch[9] = true;
            await manipulator.SwitchOutputs(outputsToSwtch);
            var (status, logicState) = await manipulator.ReadOutputs();
            System.Console.WriteLine($"status: {status}");
            for (int i = 0; i < logicState.Length; i++)
            {
                System.Console.WriteLine($"{i + 1}: {logicState[i]}");
            }
        }
    }
}
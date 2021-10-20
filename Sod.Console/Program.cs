using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sod.Core;
using Sod.Core.Communication;

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
            var sender = new SatelDataSender(new SocketSender(socket), Translation.CreateUserCodeBinaryRepresentation("6384"));
            var receiver = new SatelDataReceiver(new SocketReceiver(socket));

            var outputs = new bool[128];
            outputs[9] = true;
            await sender.SendAsync(Command.OutputsSwitch, outputs);
            var b = await receiver.ReceiveAsync();
            
            await sender.SendAsync(Command.OutputsState);
            var (_, _, logicState) = await receiver.ReceiveAsync();
            for (int i = 0; i < logicState.Length; i++)
            {
                System.Console.WriteLine($"{i + 1}: {logicState[i]}");
            }
            
            // await sender.SendAsync(Command.ZonesViolation);
            // (_, _, state) = await receiver.ReceiveAsync();
            // for (int i = 0; i < state.Length; i++)
            // {
            //     System.Console.WriteLine($"{i + 1}: {state[i]}");
            // }
        }
    }
}
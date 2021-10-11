using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sod.Core;

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
            var conn = new SatelConnection(socket);
            await conn.SendAsync(Command.OutputsState);
            var (cmd, state) = await conn.ReceiveAsync();

            for (int i = 0; i < state.Length; i++)
            {
                System.Console.WriteLine($"{i + 1}: {state[i]}");
            }
        }
    }
}
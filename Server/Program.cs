using System;
using System.Net;
using System.Threading.Tasks;
using KINetwork.DiscordBot;

namespace KINetwork
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "KI NETWORK";

            var tsk = Task.Run(() => new Server(12345).Start());

            var tsk1 = DiscordBot.DiscordBot.Load();

            await Task.WhenAll(tsk, tsk1); 
        }
    }
}

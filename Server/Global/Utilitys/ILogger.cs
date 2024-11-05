using System;
using System.IO;
using Discord;

namespace KINetwork.Global.Utilitys
{
    internal class ILogger
    {
        internal static void ILog(string msg, int type, int call)
        {
            switch (type)
            {
                case 0:
                    Console.Write(" [");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" LOG ");
                    Console.ResetColor();
                    Console.Write("] ");

                    if (call == 0)
                    {
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" SERVER ");
                        Console.ResetColor();
                        Console.Write("] ");
                    }
                    else if (call == 1)
                    {
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" DISCORD ");
                        Console.ResetColor();
                        Console.Write("] ");
                    }
                    Writeout(msg, new ConsoleColor[] { ConsoleColor.Gray, ConsoleColor.White });
                    break;
                case 1:
                    Console.Write(" [");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" WARN ");
                    Console.ResetColor();
                    Console.Write("] ");

                    if (call == 0)
                    {
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" SERVER ");
                        Console.ResetColor();
                        Console.Write("] ");
                    }
                    else if (call == 1)
                    {
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" DISCORD ");
                        Console.ResetColor();
                        Console.Write("] ");
                    }
                    Writeout(msg, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkYellow });
                    break;
                case 2:
                    Console.Write(" [");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" ERROR ");
                    Console.ResetColor();
                    Console.Write("] ");

                    if (call == 0)
                    {
                        Console.Write(" [");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" SERVER ");
                        Console.ResetColor();
                        Console.Write("] ");
                    }
                    else if (call == 1)
                    {
                        Console.Write(" [");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" DISCORD ");
                        Console.ResetColor();
                        Console.Write("] ");
                    }


                    Writeout(msg, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.DarkRed });
                    break;
            }
        }

        private static void Writeout(string msg, ConsoleColor[] colors) {
            //  WrittenLog(msg);
            for (int i = 0; i < msg.Length; i++) {
                Console.ForegroundColor = colors[(i * colors.Length) / msg.Length];
                Console.Write(msg[i]);
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}

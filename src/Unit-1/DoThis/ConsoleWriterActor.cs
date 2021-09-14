using System;
using Akka.Actor;
using Akka.IO;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for serializing message writes to the console.
    /// (write one message at a time, champ :)
    /// </summary>
    class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is Messages.InputError msg )
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg.Reason);
            }
            else if (message is Messages.InputSuccess msg1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msg1.Reason);
            }
            else
            {
                Console.WriteLine(message);
            }
            Console.ResetColor();
        }
    }
}

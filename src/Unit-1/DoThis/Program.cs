using System;
﻿using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            var consoleWriterProps = Props.Create(typeof(ConsoleWriterActor));
            var writer = MyActorSystem.ActorOf(consoleWriterProps);

            var validationActorProps = Props.Create(() => new ValidationActor(writer));
            var validator = MyActorSystem.ActorOf(validationActorProps);

            var consoleReaderProps = Props.Create(() => new ConsoleReaderActor(validator));
            var reader = MyActorSystem.ActorOf(consoleReaderProps);

            // tell console reader to begin
            reader.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }

    }
    #endregion
}

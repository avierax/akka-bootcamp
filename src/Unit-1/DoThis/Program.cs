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

            // time to make your first actors!
            var writer = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
            var validator = MyActorSystem.ActorOf(Props.Create(() => new ValidationActor(writer)));
            var reader = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(validator)));

            // tell console reader to begin
            reader.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }

    }
    #endregion
}

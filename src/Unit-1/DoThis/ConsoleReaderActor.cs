using System;
using System.Data;
using System.Diagnostics;
using Akka.Actor;
using Akka.IO;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";
        
        private IActorRef _nextActor;

        public ConsoleReaderActor(IActorRef nextActor)
        {
            _nextActor = nextActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            else if (message is Messages.InputError inputErrorMessage)
            {
                _nextActor.Tell(inputErrorMessage);
            }

            GetAndValidateInput();
        }
        #region internal methods
        private void GetAndValidateInput()
        {
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                // signal that the user needs to supply an input
                Self.Tell(new Messages.NullInputError("No input received"));
            } 
            else if (input.Equals(ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // shutdown the entire system
                Context.System.Terminate();
            }
            else
            {
                _nextActor.Tell(input);
            }
        }

        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console");
            Console.WriteLine("Some entries will pass validation");
            Console.WriteLine("Type exit to quit this application");
        }
        #endregion
    }
}
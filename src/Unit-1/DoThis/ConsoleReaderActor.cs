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
        
        private IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            else if (message is Messages.InputError inputErrorMessage)
            {
                _consoleWriterActor.Tell(inputErrorMessage);
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
                var valid = IsValid(input);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you, input was valid"));
                    Self.Tell(new Messages.ContinueProcessing());
                }
                else
                {
                    Self.Tell(new Messages.ValidationError("Invalid input: odd number of chars"));
                }
            }
        }

        private bool IsValid(string input)
        {
            return input.Length % 2 == 0;
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
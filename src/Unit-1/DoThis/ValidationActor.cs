using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace WinTail
{
    class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriterActor.Tell(new Messages.InputError("Input is null or empty"));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Input was successful"));
                }
                else
                {
                    _consoleWriterActor.Tell(
                        new Messages.ValidationError("Invalid: Input had odd number of characters"));
                }
                Sender.Tell(new Messages.ContinueProcessing());
            }
        }

        private bool IsValid(string msg)
        {
            return msg.Length % 2 == 0;
        }
    }
}

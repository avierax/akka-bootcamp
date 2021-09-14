using System;
using System.Collections.Generic;
using System.Text;

namespace WinTail.Messages
{
    #region neutral/system messages

    /// <summary>
    /// Marker class to continue processing
    /// </summary>
    public class ContinueProcessing
    {

    }
    #endregion

    #region success messages

    public class InputSuccess
    {
        public string Reason { get; }

        public InputSuccess(string reason)
        {
            Reason = reason;
        }
    }
    #endregion

    #region failure messages

    public class InputError
    {
        public string Reason { get; }

        public InputError(string reason)
        {
            Reason = reason;
        }
    }

    public class NullInputError: InputError
    {
        public NullInputError(string reason) : base(reason) {}
    }

    public class ValidationError : InputError
    {
        public ValidationError(string reason) : base(reason) {}
    }
    #endregion
}

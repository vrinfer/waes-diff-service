using System;

namespace WAES.Diff.Service.Common.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base(Constants.INVALID_INPUT_EXCEPTION_MESSAGE)
        {
        }

        public InvalidInputException(string message)
           : base(message)
        {
        }

        public InvalidInputException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

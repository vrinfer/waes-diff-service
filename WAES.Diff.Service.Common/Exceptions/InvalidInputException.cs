using System;

namespace WAES.Diff.Service.Common.Exceptions
{
    public class InvalidInputException : Exception
    {
        /// <summary>
        /// Exception thrown when the input can not be converted from base64
        /// </summary>
        public InvalidInputException() : base(Constants.INVALID_INPUT_EXCEPTION_MESSAGE)
        {
        }

        /// <summary>
        /// Exception thrown when the input can not be converted from base64
        /// </summary>
        /// <param name="message">Description of error</param>
        public InvalidInputException(string message)
           : base(message)
        {
        }
    }
}

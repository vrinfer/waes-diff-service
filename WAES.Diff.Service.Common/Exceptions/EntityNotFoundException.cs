using System;

namespace WAES.Diff.Service.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base(Constants.ENTITY_NOT_FOUND_EXCEPTION_MESSAGE)
        {
        }

        public EntityNotFoundException(string message)
           : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

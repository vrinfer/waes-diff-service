using System;

namespace WAES.Diff.Service.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Exception thrown when the entity is null, meaning ot does not found in the database
        /// </summary>
        public EntityNotFoundException() : base(Constants.ENTITY_NOT_FOUND_EXCEPTION_MESSAGE)
        {
        }

        /// <summary>
        /// Exception thrown when the entity is null, meaning ot does not found in the database
        /// </summary>
        /// <param name="message">Description of error</param>
        public EntityNotFoundException(string message)
           : base(message)
        {
        }
    }
}

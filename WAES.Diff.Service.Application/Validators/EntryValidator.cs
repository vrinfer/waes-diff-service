using WAES.Diff.Service.Common;
using WAES.Diff.Service.Common.Exceptions;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces.Validators;

namespace WAES.Diff.Service.Domain.Validators
{
    public class EntryValidator : IEntryValidator
    {
        /// <summary>
        /// Validates thar the entry object is not null
        /// Throws EntityNotFoundException if it is null
        /// </summary>
        /// <param name="entity"></param>
        public void ValidateNotNullEntity(Entry entity)
        {
            if (entity == null)
            {
                throw new EntityNotFoundException(Constants.ENTITY_NOT_FOUND_EXCEPTION_MESSAGE);
            }
        }
    }
}

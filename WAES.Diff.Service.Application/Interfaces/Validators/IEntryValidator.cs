using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Domain.Interfaces.Validators
{
    public interface IEntryValidator
    {
        void ValidateNotNullEntity(Entry entity);
    }
}

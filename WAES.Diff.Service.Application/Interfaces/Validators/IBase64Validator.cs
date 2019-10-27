using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Domain.Interfaces.Validators
{
    public interface IBase64Validator
    {
        void ValidateBase64String(string data);
        void ValidateEntry(Entry entry);
    }
}

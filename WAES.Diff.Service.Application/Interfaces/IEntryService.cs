using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Enums;

namespace WAES.Diff.Service.Domain.Interfaces
{
    public interface IEntryService
    {
        Task AddSideToCompare(Guid id, string data, Side side);
    }
}

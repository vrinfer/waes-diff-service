using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Common.Enums;
using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Domain.Interfaces
{
    public interface IDiffService
    {
        Task AddSideToCompare(Guid id, string data, Side side);
        Task<DiffResult> GetDiff(Guid id);
    }
}

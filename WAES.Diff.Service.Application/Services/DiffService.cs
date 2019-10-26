using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Common.Enums;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces;

namespace WAES.Diff.Service.Domain.Services
{
    public class DiffService : IDiffService
    {
        public Task AddSideToCompare(Guid id, string data, Side side)
        {
            throw new NotImplementedException();
        }

        public Task<DiffResult> GetDiff(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

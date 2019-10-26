using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Domain.Interfaces
{
    public interface IEntryRepository
    {
        Task Insert(Entry entry);
        Task Update(Entry entry);
        Task<Entry> GetByExternalId(Guid id);
    }
}

using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Domain.Interfaces
{
    public interface IEntryRepository
    {
        Task Upsert(Entry entry);

        Task GetByExternalId(Guid id);
    }
}

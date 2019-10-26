using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces;

namespace WAES.Diff.Service.Infrastructure.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        public Task Insert(Entry entry)
        {
            throw new NotImplementedException();
        }

        public Task Update(Entry entry)
        {
            throw new NotImplementedException();
        }

        public Task<Entry> GetByExternalId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

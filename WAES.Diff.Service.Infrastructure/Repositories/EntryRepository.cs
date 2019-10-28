using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces;

namespace WAES.Diff.Service.Infrastructure.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly DiffServiceDbContext _dbContext;

        public EntryRepository(DiffServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Insert(Entry entry)
        {
            await _dbContext.Entries.AddAsync(entry);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Entry entry)
        {
            _dbContext.Entries.Update(entry);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Entry> GetByExternalId(Guid id)
        {
            return await _dbContext.Entries.FirstOrDefaultAsync(e => e.ExternalId == id);
        }
    }
}

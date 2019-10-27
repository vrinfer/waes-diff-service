using Microsoft.EntityFrameworkCore;
using WAES.Diff.Service.Domain.Entities;

namespace WAES.Diff.Service.Infrastructure
{
    public class DiffServiceDbContext : DbContext
    {
        public DiffServiceDbContext( DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }
    }
}

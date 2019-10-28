using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces;
using WAES.Diff.Service.Infrastructure.Repositories;
using Xunit;

namespace WAES.Diff.Service.Infrastructure.Tests.Unit
{
    public class EntryRepositoryTests
    {
        public class InsertTests : EntryRepositoryTests
        {
            [Theory]
            [AutoData]
            public async Task Add_New_Entry_To_Db(Entry entry)
            {
                // Arrange
                var options = new DbContextOptionsBuilder<DiffServiceDbContext>().UseInMemoryDatabase(databaseName: "diff-service-test").Options;

                using (var context = new DiffServiceDbContext(options))
                {
                    var entryRepository = new EntryRepository(context);

                    // Act
                    await entryRepository.Insert(entry);

                    // Assert
                    using (new AssertionScope())
                    {
                        context.Entries.FindAsync(entry.Id).Should().NotBeNull();
                    }
                }
            }
        }

        public class UpdateTests : EntryRepositoryTests
        {
            [Theory]
            [AutoData]
            public async Task Updates_Entry_And_SaveChanges(Entry entry, string left, string right)
            {
                // Arrange
                var options = new DbContextOptionsBuilder<DiffServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "diff-service-test")
                .Options;

                using (var context = new DiffServiceDbContext(options))
                {
                    context.Entries.Add(entry);
                    context.SaveChanges();
                }

                using (var context = new DiffServiceDbContext(options))
                {
                    var entryRepository = new EntryRepository(context);

                    // Act
                    entry.LeftSide = left;
                    entry.RightSide = right;

                    await entryRepository.Update(entry);

                    // Assert
                    using (new AssertionScope())
                    {
                        context.Entries.FindAsync(entry.Id).Should().NotBeNull();
                    }
                }
            }
        }

        public class GetByExternalIdTests : EntryRepositoryTests
        {
            [Theory]
            [AutoData]
            public async Task Returns_Correct_Entry(Entry firstEntry, Entry secondEntry)
            {
                // Arrange
                var id = firstEntry.ExternalId;

                var options = new DbContextOptionsBuilder<DiffServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "diff-service-test")
                .Options;

                using (var context = new DiffServiceDbContext(options))
                {
                    context.Entries.Add(firstEntry);
                    context.Entries.Add(secondEntry);
                    context.SaveChanges();
                }

                using (var context = new DiffServiceDbContext(options))
                {
                    var entryRepository = new EntryRepository(context);
                    
                    // Act
                    var result = await entryRepository.GetByExternalId(id);
                    
                    // Assert
                    using(new AssertionScope())
                    {
                        result.Should().BeEquivalentTo(firstEntry);
                    }
                }
            }
        }
    }
}

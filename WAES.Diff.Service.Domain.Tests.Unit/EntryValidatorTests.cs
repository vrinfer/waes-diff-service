using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using WAES.Diff.Service.Common.Exceptions;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces.Validators;
using WAES.Diff.Service.Domain.Validators;
using Xunit;

namespace WAES.Diff.Service.Domain.Tests.Unit
{
    public class EntryValidatorTests
    {
        private readonly IEntryValidator _objectToTest;

        public EntryValidatorTests()
        {
            _objectToTest = new EntryValidator();
        }

        [Fact]
        public void Throws_Exeption_If_Entry_Is_Null()
        {
            // Act
            var ex = Record.Exception(() => _objectToTest.ValidateNotNullEntity(null));

            // Assert
            using (new AssertionScope())
            {
                ex.Should().NotBeNull();
                ex.Should().BeOfType(typeof(EntityNotFoundException));
            }
        }

        [Theory]
        [AutoData]
        public void Does_Not_Throw_Exeption_If_Entry_Is_Not_Null(Entry entry)
        {
            // Act
            var ex = Record.Exception(() => _objectToTest.ValidateNotNullEntity(entry));

            // Assert
            using (new AssertionScope())
            {
                ex.Should().BeNull();
            }
        }
    }
}

using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Domain.Interfaces;
using WAES.Diff.Service.Domain.Interfaces.Services;
using WAES.Diff.Service.Domain.Interfaces.Validators;
using WAES.Diff.Service.Domain.Services;
using Xunit;

namespace WAES.Diff.Service.Domain.Tests.Unit
{
    public class DiffServiceTests
    {
        private readonly Mock<IEntryRepository> _mockEntryRepository;
        private readonly Mock<IEntryValidator> _mockEntryValidator;
        private readonly Mock<IBase64Validator> _mockBase64Validator;
        private readonly Mock<IDiffCalculator> _mockDiffCalculator;
        private readonly DiffService _objectToTest;

        private const string ENTRY_ID = "12aaabc5-a0b0-4fe7-a5a2-ed8d65196bfe";

        public DiffServiceTests()
        {
            _mockEntryRepository = new Mock<IEntryRepository>();
            _mockEntryValidator = new Mock<IEntryValidator>();
            _mockBase64Validator = new Mock<IBase64Validator>();
            _mockDiffCalculator = new Mock<IDiffCalculator>();

            _objectToTest = new DiffService(_mockEntryRepository.Object, _mockEntryValidator.Object, _mockBase64Validator.Object, _mockDiffCalculator.Object);
        }

        private void SetupEntryRepository(Guid id, string left, string right)
        {
            var entry = new Entry
            {
                ExternalId = id,
                LeftSide = left,
                RightSide = right
            };

            _mockEntryRepository
               .Setup(x => x.GetByExternalId(It.IsAny<Guid>()))
               .ReturnsAsync(entry);
        }

        private void SetupEntryRepository(Entry entry)
        {
            _mockEntryRepository
               .Setup(x => x.GetByExternalId(It.IsAny<Guid>()))
               .ReturnsAsync(entry);
        }

        [Theory]
        [InlineData("VA==", "VAw=")]
        public async Task Gets_Entry_By_External_Id(string left, string right)
        {
            // Arrange
            var id = new Guid(ENTRY_ID);
            SetupEntryRepository(id, left, right);
            
            // Act
            await _objectToTest.GetDiff(id);

            // Assert
            _mockEntryRepository.Verify(x => x.GetByExternalId(id), Times.Once);
        }

        [Theory]
        [InlineData("VA==", "VAw=")]
        public async Task Invokes_Entry_Validator(string left, string right)
        {
            // Arrange 
            var id = new Guid(ENTRY_ID);
            var entry = new Entry
            {
                ExternalId = id,
                LeftSide = left,
                RightSide = right
            };

            SetupEntryRepository(entry);

            // Act
            await _objectToTest.GetDiff(id);

            // Assert
            _mockEntryValidator.Verify(x => x.ValidateNotNullEntity(entry), Times.Once);
        }

        [Theory]
        [InlineData("VA==", "VAw=")]
        public async Task Invokes_Base64_Validator(string left, string right)
        {
            // Arrange 
            var id = new Guid(ENTRY_ID);
            var entry = new Entry
            {
                ExternalId = id,
                LeftSide = left,
                RightSide = right
            };

            SetupEntryRepository(entry);

            // Act
            await _objectToTest.GetDiff(id);

            // Assert
            _mockBase64Validator.Verify(x => x.ValidateEntry(entry), Times.Once);
        }

        [Theory]
        [InlineData("VA==", "VAw=")]
        public async Task Returns_Unmatched_Size_Result_If_Lengths_Are_Not_Same(string left, string right)
        {
            // Arrange 
            var id = new Guid(ENTRY_ID);
            SetupEntryRepository(id, left, right);

            // Act
            var result = await _objectToTest.GetDiff(id);

            // Assert
            using(new AssertionScope())
            {
                result.Status.Should().Be(DiffStatus.UnmatchedSize);
                result.Differences.Should().BeNull();
            }
        }

        [Theory]
        [MemberData(nameof(DataForEqual))]
        public async Task Gets_Computed_Diffs_If_Sizes_Match_And_Returns_Equal(string left, string right, List<DiffDetail> differences)
        {
            // Arrange 
            var id = new Guid(ENTRY_ID);

            SetupEntryRepository(id, left, right);

            _mockDiffCalculator
                .Setup(x => x.GetComputedDiffs(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(differences);

            // Act
            var result = await _objectToTest.GetDiff(id);

            // Assert
            _mockDiffCalculator.Verify(x => x.GetComputedDiffs(It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);

            using (new AssertionScope())
            {
                result.Status.Should().Be(DiffStatus.Equal);
                result.Differences.Should().BeNull();
            }
        }


        [Theory]
        [MemberData(nameof(DataForNotEqual))]
        public async Task Gets_Computed_Diffs_If_Sizes_Match_And_Returns_Not_Equal(string left, string right, List<DiffDetail> differences)
        {
            // Arrange 
            var id = new Guid(ENTRY_ID);

            SetupEntryRepository(id, left, right);

            _mockDiffCalculator
                .Setup(x => x.GetComputedDiffs(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(differences);

            // Act
            var result = await _objectToTest.GetDiff(id);

            // Assert
            _mockDiffCalculator.Verify(x => x.GetComputedDiffs(It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);

            using (new AssertionScope())
            {
                result.Status.Should().Be(DiffStatus.NotEqual);
                result.Differences.Should().NotBeNullOrEmpty();
            }
        }

        public static TheoryData<string, string, List<DiffDetail>> DataForEqual = new TheoryData<string, string, List<DiffDetail>> {
            { "3w==", "3w==",  new List<DiffDetail>() },
        };

        public static TheoryData<string, string, List<DiffDetail>> DataForNotEqual = new TheoryData<string, string, List<DiffDetail>> {
            { "gZs=", "gZo=",  new List<DiffDetail> { new DiffDetail { Offset = 1, Length = 1 } } },
        };
    }
}

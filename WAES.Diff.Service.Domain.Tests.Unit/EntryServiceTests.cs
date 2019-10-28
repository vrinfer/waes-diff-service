using AutoFixture.Xunit2;
using Moq;
using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Domain.Interfaces;
using WAES.Diff.Service.Domain.Interfaces.Validators;
using WAES.Diff.Service.Domain.Services;
using Xunit;

namespace WAES.Diff.Service.Domain.Tests.Unit
{
    public class EntryServiceTests
    {
        private readonly Mock<IEntryRepository> _mockEntryRepository;
        private readonly Mock<IBase64Validator> _mockBase64Validator;

        private readonly EntryService _objectToTest;

        public EntryServiceTests()
        {
            _mockEntryRepository = new Mock<IEntryRepository>();
            _mockBase64Validator = new Mock<IBase64Validator>();

            _objectToTest = new EntryService(_mockEntryRepository.Object, _mockBase64Validator.Object);
        }

        [Theory]
        [AutoData]
        public async Task Invokes_Base64_Validator(Guid id, string data, Side side)
        {
            // Arrange
            await _objectToTest.AddSideToCompare(id, data, side);

            // Assert
            _mockBase64Validator.Verify(x => x.ValidateBase64String(data), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task Gets_Entity_By_ExternalId(Guid id, string data, Side side)
        {
            // Arrange
            await _objectToTest.AddSideToCompare(id, data, side);

            // Assert
            _mockEntryRepository.Verify(x => x.GetByExternalId(id), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task Invokes_Update_If_Entity_Exists(Guid id, string data, Side side, Entry entry)
        {
            // Act
            _mockEntryRepository
                .Setup(x => x.GetByExternalId(It.IsAny<Guid>()))
                .ReturnsAsync(entry);

            // Arrange
            await _objectToTest.AddSideToCompare(id, data, side);

            // Assert
            _mockEntryRepository.Verify(x => x.Update(entry), Times.Once);
            _mockEntryRepository.Verify(x => x.Insert(It.IsAny<Entry>()), Times.Never);
        }

        [Theory]
        [AutoData]
        public async Task Invokes_Insert_If_Entity_Does_Not_Exists(Guid id, string data, Side side, Entry entry)
        {
            // Arrange
            await _objectToTest.AddSideToCompare(id, data, side);

            // Assert
            _mockEntryRepository.Verify(x => x.Insert(It.IsAny<Entry>()), Times.Once);
            _mockEntryRepository.Verify(x => x.Update(It.IsAny<Entry>()), Times.Never);
        }
    }
}

using AutoFixture.Xunit2;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Domain.Interfaces.Services;
using WAES.Diff.Service.Web.Controllers;
using WAES.Diff.Service.Web.Models.Requests;
using Xunit;

namespace WAES.Diff.Service.Web.Tests.Unit
{
    public class DiffControllerTests
    {
        private readonly Mock<IDiffService> _mockDiffService;
        private readonly Mock<IEntryService> _mockEntryService;

        private readonly DiffController _objectToTest;

        public DiffControllerTests()
        {
            _mockDiffService = new Mock<IDiffService>();
            _mockEntryService = new Mock<IEntryService>();

            _objectToTest = new DiffController(_mockEntryService.Object, _mockDiffService.Object);
        }

        public class SetDiffLeftTests :  DiffControllerTests
        {
            [Theory, AutoData]
            public async Task Calls_Diff_Service_With_Correct_Parameters(DiffRequest request, Guid id)
            {
                // Act
                await _objectToTest.SetDiffLeft(id, request);

                // Assert
                _mockEntryService.Verify(x => x.AddSideToCompare(id, request.Data, Side.Left), Times.Once);
            }

            [Theory, AutoData]
            public async Task Returns_Ok_Response_With_Correct_Object(DiffRequest request, Guid id)
            {
                // Act
                var result = await _objectToTest.SetDiffLeft(id, request);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.Should().BeOfType<OkResult>();
                }
            }

            [Theory, AutoData]
            public async Task Returns_Internal_Server_Error_Response_If_Exception_Thrown(DiffRequest request, Guid id)
            {
                // Arrange
                _mockEntryService
                    .Setup(x => x.AddSideToCompare(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Side>()))
                    .Throws<Exception>();

                // Act
                var result = await _objectToTest.SetDiffLeft(id, request);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().BeOfType<ObjectResult>();

                    var objectResult = result as ObjectResult;
                    objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
                }
            }
        }

        public class SetDiffRightTests : DiffControllerTests
        {
            [Theory, AutoData]
            public async Task Calls_Diff_Service_With_Correct_Parameters(DiffRequest request, Guid id)
            {
                // Act
                await _objectToTest.SetDiffRight(id, request);

                // Assert
                _mockEntryService.Verify(x => x.AddSideToCompare(id, request.Data, Side.Right), Times.Once);
            }

            [Theory, AutoData]
            public async Task Returns_Ok_Response_With_Correct_Object(DiffRequest request, Guid id)
            {
                // Act
                var result = await _objectToTest.SetDiffRight(id, request);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.Should().BeOfType<OkResult>();
                }
            }

            [Theory, AutoData]
            public async Task Returns_Internal_Server_Error_Response_If_Exception_Thrown(DiffRequest request, Guid id)
            {
                // Arrange
                _mockEntryService
                    .Setup(x => x.AddSideToCompare(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Side>()))
                    .Throws<Exception>();

                // Act
                var result = await _objectToTest.SetDiffRight(id, request);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().BeOfType<ObjectResult>();

                    var objectResult = result as ObjectResult;
                    objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
                }
            }
        }

        public class GetDiffTests : DiffControllerTests
        {
            private void SetupDiffService(DiffResult diffResult)
            {
                _mockDiffService
                    .Setup(x => x.GetDiff(It.IsAny<Guid>()))
                    .ReturnsAsync(diffResult);
            }

            [Theory, AutoData]
            public async Task Calls_Diff_Service_With_Correct_Parameters(Guid id)
            {
                // Act
                await _objectToTest.GetDiff(id);

                // Assert
                _mockDiffService.Verify(x => x.GetDiff(id), Times.Once);
            }

            [Theory, AutoData]
            public async Task Returns_Ok_Response_With_Correct_Object(Guid id, DiffResult diffResult)
            {
                // Arrange
                SetupDiffService(diffResult);

                // Act
                var result = await _objectToTest.GetDiff(id);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.Should().BeOfType<OkObjectResult>();

                    var objectResult = result as OkObjectResult;

                    objectResult.Value.Should().BeOfType<DiffResult>();

                    var response = objectResult.Value as DiffResult;
                    response.Should().BeEquivalentTo(diffResult);
                }
            }

            [Theory, AutoData]
            public async Task Returns_Internal_Server_Error_Response_If_Exception_Thrown(Guid id)
            {
                // Arrange
                _mockDiffService
                    .Setup(x => x.GetDiff(It.IsAny<Guid>()))
                    .Throws<Exception>();

                // Act
                var result = await _objectToTest.GetDiff(id);
                
                // Assert
                using (new AssertionScope())
                {
                    result.Should().BeOfType<ObjectResult>();

                    var objectResult = result as ObjectResult;
                    objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}

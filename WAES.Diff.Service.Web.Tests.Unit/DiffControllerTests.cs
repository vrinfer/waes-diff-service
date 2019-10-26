using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Common.Enums;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces;
using WAES.Diff.Service.Web.Controllers;
using WAES.Diff.Service.Web.Models.Requests;
using WAES.Diff.Service.Web.Models.Responses;
using Xunit;

namespace WAES.Diff.Service.Web.Tests.Unit
{
    public class DiffControllerTests
    {
        private readonly Mock<IDiffService> _mockDiffService;
        private readonly Mock<IMapper> _mockMapper;

        private readonly DiffController _objectToTest;

        public DiffControllerTests()
        {
            _mockDiffService = new Mock<IDiffService>();
            _mockMapper = new Mock<IMapper>();

            _objectToTest = new DiffController(_mockDiffService.Object, _mockMapper.Object);
        }

        public class SetDiffLeftTests :  DiffControllerTests
        {
            [Theory, AutoData]
            public async Task Calls_Diff_Service_With_Correct_Parameters(DiffRequest request, Guid id)
            {
                // Act
                await _objectToTest.SetDiffLeft(id, request);

                // Assert
                _mockDiffService.Verify(x => x.AddSideToCompare(id, request.Data, Side.Left), Times.Once);
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
                _mockDiffService
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
                _mockDiffService.Verify(x => x.AddSideToCompare(id, request.Data, Side.Right), Times.Once);
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
                _mockDiffService
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
            public GetDiffTests()
            {
                SetupMapper();
            }

            private void SetupDiffService(DiffResult diffResult)
            {
                _mockDiffService
                    .Setup(x => x.GetDiff(It.IsAny<Guid>()))
                    .ReturnsAsync(diffResult);
            }

            private void SetupMapper(DiffResponse response = null)
            {
                _mockMapper
                   .Setup(x => x.Map<DiffResponse>(It.IsAny<DiffResult>()))
                   .Returns(response ?? new DiffResponse());
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
            public async Task Maps_Returned_Object_To_Response(Guid id, DiffResult diffResult)
            {
                // Arrange
                SetupDiffService(diffResult);

                // Act
                await _objectToTest.GetDiff(id);

                //Assert
                _mockMapper
                    .Verify(x => x.Map<DiffResponse>(diffResult), Times.Once);
            }

            [Theory, AutoData]
            public async Task Returns_Ok_Response_With_Correct_Object(Guid id, DiffResult diffResult, DiffResponse diffResponse)
            {
                // Arrange
                SetupDiffService(diffResult);
                SetupMapper(diffResponse);

                // Act
                var result = await _objectToTest.GetDiff(id);

                // Assert
                using (new AssertionScope())
                {
                    result.Should().NotBeNull();
                    result.Should().BeOfType<OkObjectResult>();

                    var objectResult = result as OkObjectResult;

                    objectResult.Value.Should().BeOfType<DiffResponse>();

                    var response = objectResult.Value as DiffResponse;
                    response.Should().BeEquivalentTo(diffResponse);
                }
            }

            [Theory, AutoData]
            public async Task Returns_Internal_Server_Error_Response_If_Exception_Thrown(DiffRequest request, Guid id)
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

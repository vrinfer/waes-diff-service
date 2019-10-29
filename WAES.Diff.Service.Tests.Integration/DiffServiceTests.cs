using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Web;
using WAES.Diff.Service.Web.Models.Requests;
using Xunit;

namespace WAES.Diff.Service.Tests.Integration
{
    public class DiffServiceTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        protected readonly HttpClient _httpClient;

        private const string API_LEFT_URL = "v1/diff/12aaabc5-a0b0-4fe7-a5a2-ed8d65196bfe/left";
        private const string API_RIGHT_URL = "v1/diff/12aaabc5-a0b0-4fe7-a5a2-ed8d65196bfe/right";
        private const string API_DIFF_URL = "v1/diff/12aaabc5-a0b0-4fe7-a5a2-ed8d65196bfe";


        public DiffServiceTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Theory]
        [InlineData("VA==")]
        [InlineData("SW4gdGhlIGJlZ2lubmluZyB0aGVyZSB3YXMgbm90aGluZywgd2hpY2ggZXhwbG9kZWQ=")]
        public async Task Returns_Ok_Response_On_Post_To_Set_Sides(string data)
        {
            // Arrange
            var requestLeft = new DiffRequest { Data = data };
            var requestRight = new DiffRequest { Data = data };

            // Act
            var resultLeft = await _httpClient.PostAsync(API_LEFT_URL, GetJsonHttpContent(requestRight));
            var resultRight = await _httpClient.PostAsync(API_LEFT_URL, GetJsonHttpContent(requestRight));

            // Assert
            resultLeft.EnsureSuccessStatusCode();
            resultRight.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("8T#YmBOr;qAS5^pDJ=!$+EV:.Eb-A;@<3Q/DffZ,DJ(LCGA(]#BHUl2E,9H'ARk")]
        public async Task Returns_Bad_Request_Response_On_Non_Base64_Input(string data)
        {
            // Arrange
            var requestLeft = new DiffRequest { Data = data };
            var requestRight = new DiffRequest { Data = data };

            // Act
            var resultLeft = await _httpClient.PostAsync(API_LEFT_URL, GetJsonHttpContent(requestLeft));
            var resultRight = await _httpClient.PostAsync(API_RIGHT_URL, GetJsonHttpContent(requestRight));

            // Assert
            using(new AssertionScope())
            {
                resultLeft.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
                resultRight.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            }
        }

        public static TheoryData<string, string, DiffStatus, int> DataForDiffResult = new TheoryData<string, string, DiffStatus, int> {
            { "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIDEzIGxhenkgZG9ncy4=", "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIDEzIGxhenkgZG9ncy4=",  DiffStatus.Equal, 0 },
            { "VGhlIG9ubHkgd2F0ZXIgaW4gdGhlIGZvcmVzdCBpcyB0aGUgUml2ZXI=", "Rml2ZSBleGNsYW1hdGlvbiBtYXJrcywgdGhlIHN1cmUgc2lnbiBvZiBhbiBpbnNhbmUgbWluZA==",  DiffStatus.UnmatchedSize, 0 },
            { "VGhlIG9ubHkgd2F0ZXIgaW4gdGhlIGZvcmVzdCBpcyB0aGUgUml2ZXI=", "dOjloH8uZPkw93E0dXYw6X5gfGp1oHYvemd79DApeyJ86HVgWul+5WI=",  DiffStatus.NotEqual, 1 },
            { "U29tZXRpbWVzIHlvdSB3YWtlIHVwLiBTb21ldGltZXMgdGhlIGZhbGwga2lsbHMgeW91LiBBbmQgc29tZXRpbWVzLCB3aGVuIHlvdSBmYWxsLCB5b3UgZmx5Lg==",
                "X+/tZXRpbWVzIHl/d2D34WtlIHVwLiBTb21ldGltZXMgdGhlIGZhbGwga2lsbHMgeW91LiBBbmQgc29tZXRpbWVzLCB3aGVuIHlvdSBmYWxsLCB5b3UgZmx5Lg==",  DiffStatus.NotEqual, 2 },
        };

        [Theory]
        [MemberData(nameof(DataForDiffResult))]
        public async Task Returns_Ok_Response_On_GetDiff(string left, string right, DiffStatus resultStatus, int expectedCount)
        {
            // Arrange
            var requestLeft = new DiffRequest { Data = left };
            var requestRight= new DiffRequest { Data = right };

            var responseLeft = await _httpClient.PostAsync(API_LEFT_URL, GetJsonHttpContent(requestLeft));
            responseLeft.EnsureSuccessStatusCode();
            
            var responseRight = await _httpClient.PostAsync(API_RIGHT_URL, GetJsonHttpContent(requestRight));
            responseRight.EnsureSuccessStatusCode();

            // Act
            var response = await _httpClient.GetAsync(API_DIFF_URL);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            var actualResult = JsonConvert.DeserializeObject<DiffResult>(responseString);

            // Assert
            using(new AssertionScope())
            {
                actualResult.Status.Should().Be(resultStatus);

                if(expectedCount > 0)
                {
                    actualResult.Differences.Should().NotBeNullOrEmpty();
                    actualResult.Differences.Count().Should().Be(expectedCount);
                } 
                else
                {
                    actualResult.Differences.Should().BeNull();
                }
            }
        }

        [Theory]
        [InlineData("v1/diff/00000000-a0b0-4fe7-0000-ed8d65196bfe/left", "b29wcw==", "v1/diff/00000000-a0b0-4fe7-0000-ed8d65196bfe")]
        [InlineData("v1/diff/11111111-a0b0-4fe7-1111-ed8d65196bfe/right", "b29wcw==", "v1/diff/11111111-a0b0-4fe7-1111-ed8d65196bfe")]
        public async Task Returns_Bad_Request_Response_On_GetDiff_If_Some_Side_Missing(string setSiderequestUrl, string data, string getDiffRequest)
        {
            // Arrange
            var request = new DiffRequest { Data = data };
            
            var responseCreate = await _httpClient.PostAsync(setSiderequestUrl, GetJsonHttpContent(request));
            responseCreate.EnsureSuccessStatusCode();

            // Act
            var response = await _httpClient.GetAsync(getDiffRequest);

            // Assert
            using (new AssertionScope())
            {
                response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            }
        }

        [Fact]
        public async Task Returns_Bad_Request_Response_On_GetDiff_If_Entry_Does_Not_Exists()
        {
            // Act
            var response = await _httpClient.GetAsync($"v1/diff/{Guid.NewGuid()}");

            // Assert
            using (new AssertionScope())
            {
                response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            }
        }

        [Theory]
        [InlineData("b29wcw==", "b39wcw==")]
        [InlineData("b29wcw==", "SGVscG1l")]
        public async Task Result_Changes_If_Side_Is_Updated(string initialData, string updatedData)
        {
            // Arrange
            // Setup first case to return response equal
            var initialCaseRequest = new DiffRequest { Data = initialData };

            var responseLeft = await _httpClient.PostAsync(API_LEFT_URL, GetJsonHttpContent(initialCaseRequest));
            responseLeft.EnsureSuccessStatusCode();

            var responseRight = await _httpClient.PostAsync(API_RIGHT_URL, GetJsonHttpContent(initialCaseRequest));
            responseRight.EnsureSuccessStatusCode();

            var initialDiffResponse = await _httpClient.GetAsync(API_DIFF_URL);

            // Act
            //Update left side and save response
            var updateSideRequest = new DiffRequest { Data = updatedData };

            var updatedSideResponse = await _httpClient.PostAsync(API_LEFT_URL, GetJsonHttpContent(updateSideRequest));
            updatedSideResponse.EnsureSuccessStatusCode();

            var updatedSideDiffResponse = await _httpClient.GetAsync(API_DIFF_URL);

            // Assert
            var initialResponseString = await initialDiffResponse.Content.ReadAsStringAsync();
            var initialResult = JsonConvert.DeserializeObject<DiffResult>(initialResponseString);

            var updatedResponseString = await updatedSideDiffResponse.Content.ReadAsStringAsync();
            var updatedResult = JsonConvert.DeserializeObject<DiffResult>(updatedResponseString);

            using (new AssertionScope())
            {
                initialResult.Status.Should().NotBe(updatedResult.Status);
            }
        }

        /// <summary>
        /// Creates a HttpContent from JSON serialized object
        /// </summary>
        private HttpContent GetJsonHttpContent(object value)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(value));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}

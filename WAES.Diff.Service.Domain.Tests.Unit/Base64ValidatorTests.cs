using FluentAssertions;
using FluentAssertions.Execution;
using WAES.Diff.Service.Common.Exceptions;
using WAES.Diff.Service.Domain.Interfaces.Validators;
using WAES.Diff.Service.Domain.Validators;
using Xunit;

namespace WAES.Diff.Service.Domain.Tests.Unit
{
    public class Base64ValidatorTests
    {
        private readonly IBase64Validator _objectToTest;

        public Base64ValidatorTests()
        {
            _objectToTest = new Base64Validator();
        }

        public class ValidateBase64StringTests : Base64ValidatorTests
        {
            [Theory]
            [InlineData("ul+5WI")]
            [InlineData("invalid_input")]
            public void Throws_Exeption_If_Can_Not_Convert(string input)
            {
                // Act
                var ex = Record.Exception(() => _objectToTest.ValidateBase64String(input));

                // Assert
                using (new AssertionScope())
                {
                    ex.Should().NotBeNull();
                    ex.Should().BeOfType(typeof(InvalidInputException));
                }
            }

            [Theory]
            [InlineData("Xw==")]
            [InlineData("R29kIGRvZXMgbm90IHBsYXkgZGljZSB3aXRoIHRoZSB1bml2ZXJzZS4gSGUgcGxheXMgYW4gaW5lZmZhYmxlIGdhbWUgb2YgSGlzIG93biBkZXZpc2luZywgd2hpY2ggbWlnaHQgYmUgY29tcGFyZWQsIGZyb20gdGhlIHBlcnNwZWN0aXZlIG9mIGFueSBvZiB0aGUgb3RoZXIgcGxheWVycyBpLmUuIGV2ZXJ5Ym9keSwgdG8gYmVpbmcgaW52b2x2ZWQgaW4gYW4gb2JzY3VyZSBhbmQgY29tcGxleCB2YXJpYW50IG9mIHBva2VyIGluIGEgcGl0Y2gtZGFyayByb29tLCB3aXRoIGJsYW5rIGNhcmRzLCBmb3IgaW5maW5pdGUgc3Rha2VzLCB3aXRoIGEgRGVhbGVyIHdobyB3b250IHRlbGwgeW91IHRoZSBydWxlcywgYW5kIHdobyBzbWlsZXMgYWxsIHRoZSB0aW1lLg==")]
            public void Does_Not_Throw_Exeption_If_Can_Convert(string input)
            {
                // Act
                var ex = Record.Exception(() => _objectToTest.ValidateBase64String(input));

                // Assert
                using(new AssertionScope())
                {
                    ex.Should().BeNull();
                }
            }
        }
    }
}

using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces.Services;
using WAES.Diff.Service.Domain.Services;
using Xunit;

namespace WAES.Diff.Service.Domain.Tests.Unit
{
    public class DiffCalculatorTests
    {
        private readonly IDiffCalculator _objectToTest;

        public DiffCalculatorTests()
        {
            _objectToTest = new DiffCalculator();
        }

        private byte[] GetByteArray(string data)
        {
            return Convert.FromBase64String(data);
        }

        [Theory]
        [InlineData("VA==", "VA==")]
        [InlineData("VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIDEzIGxhenkgZG9ncy4=", "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIDEzIGxhenkgZG9ncy4=")]
        [InlineData(
            "R29kIGRvZXMgbm90IHBsYXkgZGljZSB3aXRoIHRoZSB1bml2ZXJzZS4gSGUgcGxheXMgYW4gaW5lZmZhYmxlIGdhbWUgb2YgSGlzIG93biBkZXZpc2luZywgd2hpY2ggbWlnaHQgYmUgY29tcGFyZWQsIGZyb20gdGhlIHBlcnNwZWN0aXZlIG9mIGFueSBvZiB0aGUgb3RoZXIgcGxheWVycyBpLmUuIGV2ZXJ5Ym9keSwgdG8gYmVpbmcgaW52b2x2ZWQgaW4gYW4gb2JzY3VyZSBhbmQgY29tcGxleCB2YXJpYW50IG9mIHBva2VyIGluIGEgcGl0Y2gtZGFyayByb29tLCB3aXRoIGJsYW5rIGNhcmRzLCBmb3IgaW5maW5pdGUgc3Rha2VzLCB3aXRoIGEgRGVhbGVyIHdobyB3b250IHRlbGwgeW91IHRoZSBydWxlcywgYW5kIHdobyBzbWlsZXMgYWxsIHRoZSB0aW1lLg==",
            "R29kIGRvZXMgbm90IHBsYXkgZGljZSB3aXRoIHRoZSB1bml2ZXJzZS4gSGUgcGxheXMgYW4gaW5lZmZhYmxlIGdhbWUgb2YgSGlzIG93biBkZXZpc2luZywgd2hpY2ggbWlnaHQgYmUgY29tcGFyZWQsIGZyb20gdGhlIHBlcnNwZWN0aXZlIG9mIGFueSBvZiB0aGUgb3RoZXIgcGxheWVycyBpLmUuIGV2ZXJ5Ym9keSwgdG8gYmVpbmcgaW52b2x2ZWQgaW4gYW4gb2JzY3VyZSBhbmQgY29tcGxleCB2YXJpYW50IG9mIHBva2VyIGluIGEgcGl0Y2gtZGFyayByb29tLCB3aXRoIGJsYW5rIGNhcmRzLCBmb3IgaW5maW5pdGUgc3Rha2VzLCB3aXRoIGEgRGVhbGVyIHdobyB3b250IHRlbGwgeW91IHRoZSBydWxlcywgYW5kIHdobyBzbWlsZXMgYWxsIHRoZSB0aW1lLg==")]
        public void Returns_Empty_Diff_List(string left, string right)
        {
            // Arrange 
            var leftByteArray = GetByteArray(left);
            var rightByteArray = GetByteArray(right);

            // Act
            var result = _objectToTest.GetComputedDiffs(leftByteArray, rightByteArray);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeEmpty();
            }
        }

        public static TheoryData<string, string, List<DiffDetail>> DataForDifferences = new TheoryData<string, string, List<DiffDetail>> {
            { "gZs=", "gZo=",  new List<DiffDetail> { new DiffDetail { Offset = 1, Length = 1 } } },
            { "3w==", "Xw==",  new List<DiffDetail> { new DiffDetail { Offset = 0, Length = 1 } } },
            { "U29tZXRpbWVzIHlvdSB3YWtlIHVwLiBTb21ldGltZXMgdGhlIGZhbGwga2lsbHMgeW91LiBBbmQgc29tZXRpbWVzLCB3aGVuIHlvdSBmYWxsLCB5b3UgZmx5Lg==",
               "X+/tZXRpbWVzIHl/d2D34WtlIHVwLiBTb21ldGltZXMgdGhlIGZhbGwga2lsbHMgeW91LiBBbmQgc29tZXRpbWVzLCB3aGVuIHlvdSBmYWxsLCB5b3UgZmx5Lg==",
                new List<DiffDetail>{ new DiffDetail { Offset = 0, Length = 3 }, new DiffDetail { Offset = 11, Length = 5 } } },
            { "RmFpcnkgdGFsZXMgYXJlIG1vcmUgdGhhbiB0cnVlIG5vdCBiZWNhdXNlIHRoZXkgdGVsbCB1cyB0aGF0IGRyYWdvbnMgZXhpc3QsIGJ1dCBiZWNhdXNlIHRoZXkgdGVsbCB1cyB0aGF0IGRyYWdvbnMgY2FuIGJlIGJlYXRlbg==",
               "RnFpcnkgdGFsZXM8eRLlJg1fQuUwdGhhbiB0cnVlIG5vdCBiZWNhdXNlIHRoZXkgdGVsbCB1cyB0aGF0IGRyYWdvbnMgZXhpc3QsIGJ1dCBiZWNhdXNlIHRoZXkgdGVsbCB1cyB0aGF0IGRyYWdvbnMgY2FuIGJlIGJlYXRNeg==",
                new List<DiffDetail>{ new DiffDetail { Offset = 1, Length = 1 }, new DiffDetail { Offset = 11, Length = 10 }, new DiffDetail { Offset = 125, Length = 2 } } },
            { "VGhlIG9ubHkgd2F0ZXIgaW4gdGhlIGZvcmVzdCBpcyB0aGUgUml2ZXI=", "dOjloH8uZPkw93E0dXYw6X5gfGp1oHYvemd79DApeyJ86HVgWul+5WI=",  new List<DiffDetail> { new DiffDetail { Offset = 0, Length = 41 } } },
        };

        [Theory]
        [MemberData(nameof(DataForDifferences))]
        public void Returns_Correct_Differences(string left, string right, List<DiffDetail> differences)
        {
            // Arrange 
            var leftByteArray = GetByteArray(left);
            var rightByteArray = GetByteArray(right);

            // Act
            var result = _objectToTest.GetComputedDiffs(leftByteArray, rightByteArray);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeEmpty();
                result.Should().BeEquivalentTo(differences);
            }
        }
    }
}

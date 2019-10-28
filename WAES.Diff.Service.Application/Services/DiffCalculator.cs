using System.Collections;
using System.Collections.Generic;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces.Services;

namespace WAES.Diff.Service.Domain.Services
{
    public class DiffCalculator : IDiffCalculator
    {
        public List<DiffDetail> GetComputedDiffs(byte[] left, byte[] right)
        {
            var result = new List<DiffDetail>();

            var index = 0;

            while (index < left.Length)
            {
                var equivalent = left[index] == right[index];
                if (!equivalent)
                {
                    (var lastIndex, DiffDetail diff) = AddDiffToResult(left, right, index);

                    result.Add(diff);
                    index = lastIndex;
                }

                index++;
            }

            return result;
        }

        private (int index, DiffDetail diffDetail) AddDiffToResult(byte[] left, byte[] right, int index)
        {
            var diff = new DiffDetail
            {
                Offset = index,
                Length = 0
            };

            while (index < left.Length && left[index] != right[index])
            {
                diff.Length++;
                index++;
            }

            return (index, diff);
        }
    }
}

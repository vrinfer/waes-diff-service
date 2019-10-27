using System.Collections;
using System.Collections.Generic;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces.Services;

namespace WAES.Diff.Service.Domain.Services
{
    public class DiffCalculator : IDiffCalculator
    {
        public List<DiffDetail> GetComputedDiffs(BitArray left, BitArray right)
        {
            var result = new List<DiffDetail>();

            var index = 0;

            while (index < left.Count)
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

        private (int index, DiffDetail diffDetail) AddDiffToResult(BitArray left, BitArray right, int index)
        {
            var diff = new DiffDetail
            {
                Offset = index,
                Length = 0
            };

            while (index < left.Count && left[index] != right[index])
            {
                diff.Length++;
                index++;
            }

            return (index, diff);
        }
    }
}

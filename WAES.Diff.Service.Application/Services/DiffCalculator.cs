using System.Collections.Generic;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces.Services;

namespace WAES.Diff.Service.Domain.Services
{
    public class DiffCalculator : IDiffCalculator
    {
        /// <summary>
        /// Compares buth byte arrays and returns a collection of each difference offset and length
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IEnumerable<DiffDetail> GetComputedDiffs(byte[] left, byte[] right)
        {
            var result = new List<DiffDetail>();

            var index = 0;

            while (index < left.Length)
            {
                //Loops untill it finds two different bytes at the same index
                var equivalent = left[index] == right[index];
                if (!equivalent)
                {
                    //Once it founds a difference it gets amount of different consecutive bytes to create a result and returns the index of the next equal byte
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

            //Loops untill it finds two equal bytes at the same index
            while (index < left.Length && left[index] != right[index])
            {
                diff.Length++;
                index++;
            }

            return (index, diff);
        }
    }
}

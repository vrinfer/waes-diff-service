using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAES.Diff.Service.Common;
using WAES.Diff.Service.Common.Exceptions;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Domain.Helpers;
using WAES.Diff.Service.Domain.Interfaces;

namespace WAES.Diff.Service.Domain.Services
{
    public class DiffService : IDiffService
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IDiffCalculator _diffCalculator;

        public DiffService(IEntryRepository entryRepository, IDiffCalculator diffCalculator)
        {
            _entryRepository = entryRepository;
            _diffCalculator = diffCalculator;
        }

        public async Task<DiffResult> GetDiff(Guid id)
        {
            var entity = await _entryRepository.GetByExternalId(id);

            ValidateEntity(entity);

            var leftBitArray = Base64Helper.GetBitArray(entity.LeftSide);
            var rightBitArray = Base64Helper.GetBitArray(entity.RightSide);

            if (leftBitArray.Count != rightBitArray.Length)
            {
                return CreateUnmatchedSizeResult();
            }

            List<DiffDetail> diffs = _diffCalculator.GetComputedDiffs(leftBitArray, rightBitArray);
            
            return CreateComputedResult(diffs);
        }

        private static DiffResult CreateComputedResult(List<DiffDetail> diffs)
        {
            return new DiffResult
            {
                Status = diffs.Any() ? DiffStatus.NotEqual : DiffStatus.Equal,
                Differences = diffs
            };
        }

        private static DiffResult CreateUnmatchedSizeResult()
        {
            return new DiffResult
            {
                Status = DiffStatus.UnmatchedSize
            };
        }

        private void ValidateEntity(Entry entity)
        {
            if(entity == null)
            {
                throw new EntityNotFoundException(Constants.ENTITY_NOT_FOUND_EXCEPTION_MESSAGE);
            }
        }
    }
}

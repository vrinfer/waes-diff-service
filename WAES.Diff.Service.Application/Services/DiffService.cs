using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Domain.Interfaces;
using WAES.Diff.Service.Domain.Interfaces.Services;
using WAES.Diff.Service.Domain.Interfaces.Validators;

namespace WAES.Diff.Service.Domain.Services
{
    public class DiffService : IDiffService
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IEntryValidator _entryValidator;
        private readonly IBase64Validator _base64Validator;
        private readonly IDiffCalculator _diffCalculator;

        public DiffService(IEntryRepository entryRepository, IEntryValidator entryValidator, IBase64Validator base64Validator, IDiffCalculator diffCalculator)
        {
            _entryRepository = entryRepository;
            _entryValidator = entryValidator;
            _base64Validator = base64Validator;
            _diffCalculator = diffCalculator;
        }

        public async Task<DiffResult> GetDiff(Guid id)
        {
            var entry = await _entryRepository.GetByExternalId(id);

            _entryValidator.ValidateNotNullEntity(entry);
            _base64Validator.ValidateEntry(entry);

            var leftBitArray = GetBitArray(entry.LeftSide);
            var rightBitArray = GetBitArray(entry.RightSide);

            if (leftBitArray.Count != rightBitArray.Length)
            {
                return new DiffResult(DiffStatus.UnmatchedSize);
            }

            List<DiffDetail> diffs = _diffCalculator.GetComputedDiffs(leftBitArray, rightBitArray);
            
            return new DiffResult(diffs);
        }

        private BitArray GetBitArray(string data)
        {
            var bytes = Convert.FromBase64String(data);

            return new BitArray(bytes);
        }
    }
}

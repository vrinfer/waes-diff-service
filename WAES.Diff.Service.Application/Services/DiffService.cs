using System;
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

            var leftByteArray = GetByteArray(entry.LeftSide);
            var rightByteArray = GetByteArray(entry.RightSide);

            if (leftByteArray.Length != rightByteArray.Length)
            {
                return new DiffResult(DiffStatus.UnmatchedSize);
            }

            var diffs = _diffCalculator.GetComputedDiffs(leftByteArray, rightByteArray);
            
            return new DiffResult(diffs);
        }

        private byte[] GetByteArray(string data)
        {
            return Convert.FromBase64String(data);
        }
    }
}

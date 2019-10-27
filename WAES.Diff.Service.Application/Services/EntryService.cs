using System;
using System.Threading.Tasks;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Enums;
using WAES.Diff.Service.Domain.Interfaces;
using WAES.Diff.Service.Domain.Interfaces.Services;
using WAES.Diff.Service.Domain.Interfaces.Validators;

namespace WAES.Diff.Service.Domain.Services
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IBase64Validator _base64Validator;

        public EntryService(IEntryRepository entryRepository, IBase64Validator base64Validator)
        {
            _entryRepository = entryRepository;
            _base64Validator = base64Validator;
        }

        public async Task AddSideToCompare(Guid id, string data, Side side)
        {
            _base64Validator.ValidateBase64String(data);

            var entry = await _entryRepository.GetByExternalId(id);

            if (entry != null)
            {
                await UpdateEntry(data, side, entry);
            }

            await InsertNewEntry(id, data, side);
        }

        private async Task InsertNewEntry(Guid id, string data, Side side)
        {
            Entry entry = new Entry
            {
                ExternalId = id,
                LeftSide = side == Side.Left ? data : string.Empty,
                RightSide = side == Side.Right ? data : string.Empty,
            };

            await _entryRepository.Insert(entry);
        }

        private async Task UpdateEntry(string data, Side side, Entry entry)
        {
            if (side == Side.Left) entry.LeftSide = data;
            entry.RightSide = data;

            await _entryRepository.Update(entry);
        }
    }
}

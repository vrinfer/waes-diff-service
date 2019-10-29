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

        /// <summary>
        /// Adds or Updates an entry.
        /// </summary>
        /// <param name="id">Identifies an entry which contains the fields to compare</param>
        /// <param name="data">string representig base64 encoded data</param>
        /// <param name="side">Identifies the field to update (right or left)</param>
        /// <returns></returns>
        public async Task AddSideToCompare(Guid id, string data, Side side)
        {
            _base64Validator.ValidateBase64String(data);

            var entry = await _entryRepository.GetByExternalId(id);

            if (entry != null)
            {
                await UpdateEntry(data, side, entry);
            }
            else
            {
                await InsertNewEntry(id, data, side);
            }
        }

        /// <summary>
        /// Creates and inserts the entry
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        private async Task InsertNewEntry(Guid id, string data, Side side)
        {
            Entry entry = new Entry
            {
                ExternalId = id,
                LeftSide = side == Side.Left ? data : null,
                RightSide = side == Side.Right ? data : null,
            };

            await _entryRepository.Insert(entry);
        }

        /// <summary>
        /// Updates and existing entry and impacts it in the DB
        /// </summary>
        /// <param name="data"></param>
        /// <param name="side"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private async Task UpdateEntry(string data, Side side, Entry entry)
        {
            if (side == Side.Left)
            {
                entry.LeftSide = data;
            }
            else
            {
                entry.RightSide = data;
            }

            await _entryRepository.Update(entry);
        }
    }
}

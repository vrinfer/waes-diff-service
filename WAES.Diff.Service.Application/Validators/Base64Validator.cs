using System;
using WAES.Diff.Service.Common;
using WAES.Diff.Service.Common.Exceptions;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces.Validators;

namespace WAES.Diff.Service.Domain.Validators
{
    public class Base64Validator : IBase64Validator
    {
        /// <summary>
        /// Validates if the string can be converted from base64 to byte array
        /// Throws InvalidInputException if it is not possible
        /// </summary>
        /// <param name="data"></param>
        public void ValidateBase64String(string data)
        {
            try
            {
                Convert.FromBase64String(data);
            }
            catch (Exception)
            {
                throw new InvalidInputException(Constants.INVALID_INPUT_EXCEPTION_MESSAGE);
            }
        }

        /// <summary>
        /// Validates if both LeftSide and RightSide can be converted from base64 to byte array
        /// Throws InvalidInputException if any of them fails
        /// </summary>
        /// <param name="entry"></param>
        public void ValidateEntry(Entry entry)
        {
            ValidateBase64String(entry.LeftSide);
            ValidateBase64String(entry.RightSide);
        }
    }
}

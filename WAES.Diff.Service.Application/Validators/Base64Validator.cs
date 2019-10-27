using System;
using WAES.Diff.Service.Common.Exceptions;
using WAES.Diff.Service.Domain.Entities;
using WAES.Diff.Service.Domain.Interfaces.Validators;

namespace WAES.Diff.Service.Domain.Validators
{
    public class Base64Validator : IBase64Validator
    {
        public void ValidateBase64String(string data)
        {
            try
            {
                Convert.FromBase64String(data);
            }
            catch (Exception)
            {
                throw new InvalidInputException();
            }
        }

        public void ValidateEntry(Entry entry)
        {
            ValidateBase64String(entry.LeftSide);
            ValidateBase64String(entry.RightSide);
        }
    }
}

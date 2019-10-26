using System;
using System.Collections;

namespace WAES.Diff.Service.Domain.Helpers
{
    public class Base64Helper
    {
        public static bool IsValidInput(string data)
        {
            try
            {
                Convert.FromBase64String(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static BitArray GetBitArray(string data)
        {
            try
            {
                var bytes = Convert.FromBase64String(data);

                return new BitArray(bytes);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

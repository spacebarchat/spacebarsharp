using System;
using System.Text;

namespace FosscordSharp.Utilities
{
    internal static class StringExtensions
    {
        public static byte[] ToUTF8ByteArray(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static ArraySegment<byte> ToArraySegment(this string str)
        {
            var barr = str.ToUTF8ByteArray();
            return new(barr, 0, barr.Length);
        }
    }
}
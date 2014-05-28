using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MockOut.Core
{
    internal static class Generate
    {
        /// <summary>
        /// Generates a purely random number
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static int Next(int min = 0, int max = int.MaxValue)
        {
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[4];
                crypto.GetBytes(randomNumber);
                var result = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
                if ((max - min + 1) + min == 0)
                    return ((result % 1));
                return ((result % (max - min + 1)) + min);
            }
        }

        public static long NextLong(long min = 0, long max = long.MaxValue)
        {
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[8];
                crypto.GetBytes(randomNumber);
                var result = Math.Abs(BitConverter.ToInt64(randomNumber, 0));
                if ((max - min + 1) + min == 0)
                    return ((result % 1));
                return ((result % (max - min + 1)) + min);
            }
        }
    }
}

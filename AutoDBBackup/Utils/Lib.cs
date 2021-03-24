using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDBBackup.Utils
{
    public class Lib
    {
        /// <summary>
        /// Gets the SHA512 hash of a string
        /// (from https://stackoverflow.com/questions/11367727/how-can-i-sha512-a-string-in-c)
        /// </summary>
        /// <param name="input">The string to be hashed</param>
        /// <returns>The hashed version of the string</returns>
        public static string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
    }
}

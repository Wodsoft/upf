using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal static class HashFn
    {
        // Small prime number used as a multiplier in the supplied hash functions
        private const int _HASH_MULTIPLIER = 101;

        internal static int HashMultiply(int hash)
        {
            return hash * _HASH_MULTIPLIER;
        }

        /// <summary>
        /// Distributes accumulated hash value across a range.
        /// Should be called just before returning hash code from a hash function.
        /// </summary>
        /// <param name="hash">Hash value</param>
        /// <returns>Scrambed hash value</returns>
        internal static int HashScramble(int hash)
        {
            // Here are 10 primes slightly greater than 10^9
            //  1000000007, 1000000009, 1000000021, 1000000033, 1000000087,
            //  1000000093, 1000000097, 1000000103, 1000000123, 1000000181.

            // default value for "scrambling constant"
            const int random_CONSTANT = 314159269;
            // large prime number, also used for scrambling
            const uint random_PRIME = 1000000007;

            // we must cast to uint and back to int to correspond to current C++ behavior for operator%
            // since we have a matching hash function in native code
            uint a = (uint)(random_CONSTANT * hash);
            int b = (int)(a % random_PRIME);
            return b;
        }

        /// <summary>
        /// Computes a hash code for a block of memory.
        /// One should not forget to call HashScramble before returning the final hash code value to the client.
        /// </summary>
        /// <param name="pv">Pointer to a block of memory</param>
        /// <param name="numBytes">Size of the memory block in bytes</param>
        /// <param name="hash">Previous hash code to combine with</param>
        /// <returns>Hash code</returns>
        internal unsafe static int HashMemory(void* pv, int numBytes, int hash)
        {
            byte* pb = (byte*)pv;

            while (numBytes-- > 0)
            {
                hash = HashMultiply(hash) + *pb;
                ++pb;
            }

            return hash;
        }

        internal static int HashString(string s, int hash)
        {
            foreach (char c in s)
            {
                hash = HashMultiply(hash) + (ushort)c;
            }
            return hash;
        }
    }
}

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServerHelper
{
    internal static class SecretGenerator
    {
        private const int SIZE = 64;

        private static readonly char[] _chars;

        static SecretGenerator()
        {
            var integers = Enumerable.Empty<int>();
            integers = integers.Concat(Enumerable.Range('0', 10));
            integers = integers.Concat(Enumerable.Range('A', 26));
            integers = integers.Concat(Enumerable.Range('a', 26));

            _chars = integers.Select(i => (char)i).ToArray();
        }

        public static string Generate()
        {
            var data = new byte[4 * SIZE];

            Console.WriteLine("a: {0}", (int)'a');
            Console.WriteLine("z: {0}", (int)'z');
            Console.WriteLine("A: {0}", (int)'A');
            Console.WriteLine("Z: {0}", (int)'Z');
            Console.WriteLine("0: {0}", (int)'0');
            Console.WriteLine("9: {0}", (int)'9');

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }
            var builder = new StringBuilder(SIZE);
            for (int i = 0; i < SIZE; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % _chars.Length;

                builder.Append(_chars[idx]);
            }

            return builder.ToString();
        }
    }
}

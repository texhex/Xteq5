using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yamua
{
    /// <summary>
    /// Helper class for generating random values.
    /// Created by [Jonas John](http://www.jonasjohn.de/) - http://www.jonasjohn.de/snippets/csharp/random-helper-class.htm
    /// </summary>
    public static class RandomHelper
    {
        private static Random randomSeed = new Random();

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        public static string RandomString(int size, bool lowerCase)
        {
            // StringBuilder is faster than using strings (+=)
            StringBuilder RandStr = new StringBuilder(size);

            // Ascii start position (65 = A / 97 = a)
            int Start = (lowerCase) ? 97 : 65;

            // Add random chars
            for (int i = 0; i < size; i++)
                RandStr.Append((char)(26 * randomSeed.NextDouble() + Start));

            return RandStr.ToString();
        }

        /// <summary>
        /// Returns a random number.
        /// </summary>
        /// <param name="min">Minimal result</param>
        /// <param name="max">Maximal result</param>
        /// <returns>Random number</returns>
        public static int RandomNumber(int minimal, int maximal)
        {
            return randomSeed.Next(minimal, maximal);
        }

        /// <summary>
        /// Returns a random boolean value
        /// </summary>
        /// <returns>Random boolean value</returns>
        public static bool RandomBool()
        {
            return (randomSeed.NextDouble() > 0.5);
        }


    }


}

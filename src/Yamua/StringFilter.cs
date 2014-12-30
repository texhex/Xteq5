using System;
using System.Collections.Generic;
using System.Text;

namespace Yamua
{
    public static class ASCIIStringFilter
    {
        public const string CHARS_LOWER_CASE = "abcdefghijklmnopqrstuvwxyz";
        public const string CHARS_UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string CHARS_NUMERIC = "0123456789";
        public const string CHAR_MINUS = "-";
        public const string CHAR_UNDERSCORE = "_";
        


        /// <summary>
        /// This function will filter (remove) all chars that are NOT INCLUDED in the given AllowedChars parameter.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="AllowedChars"></param>
        /// <returns></returns>
        public static string Filter(string Input, string AllowedChars)
        {
            int iPos = -1;
            StringBuilder sbResult = new StringBuilder();

            foreach (char ch in Input.ToCharArray())
            {
                iPos = AllowedChars.IndexOf(ch);
                if (iPos > -1)
                {
                    sbResult.Append(ch);
                }
            }
            return sbResult.ToString();
        }

        /// <summary>
        /// This function will ONLY remove the chars in the RemoveChars string but leave all other in place
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="RemoveChars"></param>
        /// <returns></returns>
        public static string Remove(string Input, string RemoveChars)
        {
            int iPos = -1;
            StringBuilder sbResult = new StringBuilder();

            foreach (char ch in Input.ToCharArray())
            {
                iPos = RemoveChars.IndexOf(ch);
                if (iPos == -1)
                {
                    sbResult.Append(ch);
                }
            }
            
            return sbResult.ToString();
        }




    }
}

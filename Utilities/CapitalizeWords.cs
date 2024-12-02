using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Utilities
{
    public static class CapitalizeWords
    {
        public static string CapitalizeEachWords(this string originalString)
        {
            if (string.IsNullOrEmpty(originalString))
            {
                return originalString;
            }

            var capitalizedWords = originalString.ToLower().Split(' ').Select(word => word.CapitalizeWord()).ToArray();

            return string.Join(" ", capitalizedWords);
        }

        public static string CapitalizeWord(this string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return word;
            }

            return char.ToUpper(word[0]) + word[1..];
        }
    }
}

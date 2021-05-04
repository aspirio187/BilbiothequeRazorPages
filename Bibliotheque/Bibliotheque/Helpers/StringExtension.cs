using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.Helpers
{
    public static class StringExtension
    {
        public static string FirstCharToUpper(this string input)
        {
            if (input is null) return string.Empty;
            input = input.Trim();
            if (input.Length == 0) return string.Empty;
            if (input.Length == 1) return input.ToUpper();
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string FirstCharToUpperForEach(this string input, char seperator)
        {
            StringBuilder stringBuilder = new();
            string[] seperatedInput = input.Split(seperator);
            for (int i = 0; i < seperatedInput.Length; i++)
            {
                seperatedInput[i] = seperatedInput[i].FirstCharToUpper();
                if ((i + 1) < seperatedInput.Length)
                {
                    stringBuilder.Append($"{seperatedInput[i]}{seperator}");
                }
                else
                {
                    stringBuilder.Append(seperatedInput[i]);
                }
            }
            return stringBuilder.ToString();
        }

        public static string FirstCharToUpperForEach(this string input, char[] seperator)
        {
            string output = input;
            for (int i = 0; i < seperator.Length; i++)
            {
                output = FirstCharToUpperForEach(output, seperator[i]);
            }
            return output;
        }

        public static bool IsDigit(this string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i])) return false;
            }
            return true;
        }
    }
}

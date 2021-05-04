using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotheque.Data
{
    public static class BookStates
    {
        public const string NEW = "Neuf";
        public const string LIKE_NEW = "Comme neuf";
        public const string EXCELLENT = "Excellent";
        public const string VERY_GOOD = "Très bon";
        public const string GOOD = "Bon";
        public const string ACCEPTABLE = "Acceptable";
        public const string OUT_OF_USAGE = "Hors d'usage";

        public static string[] GetBookStates()
        {
            return new string[]
            {
                 NEW,
                 LIKE_NEW,
                 EXCELLENT,
                 VERY_GOOD,
                 GOOD,
                 ACCEPTABLE,
                 OUT_OF_USAGE,
            };
        }

        public static bool StateExist(string state)
        {
            return GetBookStates().Any(x => x.Equals(state));
        }
    }
}

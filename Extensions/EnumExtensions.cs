using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<int> GetEnumValues(Type enumType)
        {
            return Enum.GetValues(enumType).Cast<int>().OrderBy(x => x);
        }
    }
}

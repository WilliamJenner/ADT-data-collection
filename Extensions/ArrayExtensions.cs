using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class ArrayExtensions
    {
        public static bool IndexExists<T>(this T[] array, int index)
        {
            return array.ElementAtOrDefault(index) != null;
        }
    }
}

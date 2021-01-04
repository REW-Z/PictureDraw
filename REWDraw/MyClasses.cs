using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REWDraw
{
    public static class MyClasses
    {
        public static T Clamp<T>(this T value, T min, T max) where T:IComparable
        {
            T newValue = value;
            if (newValue.CompareTo(min) < 0) newValue = min;
            if (newValue.CompareTo(max) > 0) newValue = max;
            return newValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    public static class NaturalSort
    {
        /* Examples From [Natural Sorting in C#](http://www.interact-sw.co.uk/iangblog/2007/12/13/natural-sorting) by Ian Griffiths
         * 
         * string[] testItems = { "z24", "z2", "z15", "z1", "z3", "z20", "z5", "z11", "z 21", "z22" };
         * NaturalSort.Sort(testItems);
         */
        public static void Sort(string[] array)
        {
            Array.Sort(array, new AlphanumComparator.AlphanumComparator());
        }
    }
}

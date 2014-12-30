using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Yamua;

namespace Yamua_xUnitTest
{
    public class StringTokenizer_Test
    {
        [Fact]
        public void StringTokenizer_Test1()
        {
            StringTokenizer st = new StringTokenizer("This is a sentence", ' ');
            Assert.Equal<int>(st.Length, 4);

        }

        [Fact]
        public void StringTokenizer_Test2()
        {
            StringTokenizer st = new StringTokenizer("This", ' ');
            Assert.Equal<int>(st.Length, 1);

        }

    }
}

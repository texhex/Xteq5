using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamua;
using Xunit;

namespace Yamua_xUnitTest
{
    public class ASCIIStringFilter_Test
    {
        [Fact]
        void ASCIIStringFilter_Test1()
        {
            string sResult = ASCIIStringFilter.Filter("ABCefgh", ASCIIStringFilter.CHARS_UPPER_CASE + ASCIIStringFilter.CHARS_LOWER_CASE);
            Assert.Equal<string>("ABCefgh", sResult);
        }

        [Fact]
        void ASCIIStringFilter_Test2()
        {
            string sResult = ASCIIStringFilter.Filter("123ABC4567efgh890  ", ASCIIStringFilter.CHARS_NUMERIC);
            Assert.Equal<string>("1234567890", sResult);
        }

        [Fact]
        void ASCIIStringFilter_Test3()
        {
            string sResult = ASCIIStringFilter.Filter("123ABC4567efgh890  ", ASCIIStringFilter.CHARS_LOWER_CASE);
            Assert.Equal<string>("efgh", sResult);
        }

        [Fact]
        void ASCIIStringFilter_Test4()
        {
            string sResult = ASCIIStringFilter.Filter("123ABC4567efgh890  ", ASCIIStringFilter.CHARS_UPPER_CASE);
            Assert.Equal<string>("ABC", sResult);
        }

        [Fact]
        void ASCIIStringFilter_Test5()
        {
            string sResult = ASCIIStringFilter.Filter("\r123A\nBC4567efgh  890^^``Z", ASCIIStringFilter.CHARS_UPPER_CASE);
            Assert.Equal<string>("ABCZ", sResult);
        }

    }
}

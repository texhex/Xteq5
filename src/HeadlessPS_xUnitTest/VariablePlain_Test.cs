using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using HeadlessPS;

namespace HeadlessPS_xUnitTest
{
    public class VariablePlain_Test
    {
        [Fact]
        public void VariablePlain_Test__Name()
        {
            string test = "NameFromConstructor";

            VariablePlain vp = new VariablePlain(test);
            Assert.Equal(test, vp.Name);

            vp.Name = " Special \r\n  ö$()[]- Case ?  ";
            Assert.Equal("SpecialCase", vp.Name);
        }

        [Fact]
        public void VariablePlain_Test__Equal()
        {
            VariablePlain vp = new VariablePlain("Name One");
            VariablePlain vp2 = new VariablePlain("NameOne");

            Assert.Equal(true, vp.Equals(vp2));
        }

    }
}

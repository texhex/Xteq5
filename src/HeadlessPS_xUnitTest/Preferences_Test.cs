using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using HeadlessPS;
using System.IO;


namespace HeadlessPS_xUnitTest
{
    public class Preferences_Test
    {
    
        [Fact]
        public void Preferences_Test__ModulePath()
        {
            PSScriptRunnerPreferences prefs = new PSScriptRunnerPreferences();
            
            //Exception ex = 
            Assert.Throws<ArgumentNullException>(() => prefs.ModulePath=null);

            Assert.Throws<DirectoryNotFoundException>(() => prefs.ModulePath = @"Z:\yyy\xxx\not\existing\path\");

            //Show work
            prefs.ModulePath = "";
            
        }

        [Fact]
        public void Preferences_Test__PSVersion()
        {
            PSScriptRunnerPreferences prefs = new PSScriptRunnerPreferences();

            Assert.Throws<ArgumentOutOfRangeException>(() => prefs.RequiredPSVersion = 2);

            Assert.Throws<ArgumentOutOfRangeException>(() => prefs.RequiredPSVersion = -5);

            Assert.Throws<ArgumentOutOfRangeException>(() => prefs.RequiredPSVersion = 0);

            //Should work
            prefs.RequiredPSVersion = 4;

        }
    }
}

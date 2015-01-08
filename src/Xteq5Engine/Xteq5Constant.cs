using System;
using System.Reflection;

namespace Xteq5
{

    public static class Xteq5Constant
    {
        public static Version AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        //MTH: These are static readonly fields BY PURPOSE (not CONST) to avoid that the compiler performs an copy+paste to the assembly of the consumer.
        //See [Const Strings - a very convenient way to shoot yourself in the foot](http://www.stum.de/2009/01/14/const-strings-a-very-convenient-way-to-shoot-yourself-in-the-foot/) by [Michael Stum](http://www.stum.de/)        

        public static readonly string ScriptFilePattern = "*.ps1";

        public static readonly string DirectoryNameAssets = "assets";
        public static readonly string DirectoryNameTests = "tests";
        public static readonly string DirectoryNameModules = "modules";

        public static readonly string VariableNameEngineVersion = "Xteq5EngineVersion";
        public static readonly string VariableNameAssets = "Xteq5Assets";
        public static readonly string VariableNameIsActive = "Xteq5Active";

        public static readonly string DirectoryNameCommonApplicationData = "Xteq5";

        //These are const because nobody outside this assembly can use them anyway
        internal const string ReturnedHashtableKeyName = "Name";
        internal const string ReturnedHashtableKeyData = "Data";
        internal const string ReturnedHashtableKeyText = "Text";

    }

}

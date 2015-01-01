using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamua;

namespace HeadlessPS
{    

    /// <summary>
    /// A minimalistic object for PowerShell variables. Note that trying to add a new VariablePlain with the same name as an existing entry 
    /// will result in FALSE beeing returned by HashSet.Add() and the new variable will not be added.
    /// To create a collection for VariablePlain, use a hash set like this: HashSet<VariablePlain> variables = new HashSet<VariablePlain>();
    /// </summary>
    public class VariablePlain : IEquatable<VariablePlain> //IEquatable to make sure that objects with the same .Name are considered to be equal (Interface is used by HashSet)
    {
        /// <summary>
        /// Create an empty variable
        /// </summary>
        public VariablePlain()
        {

        }

        /// <summary>
        /// Creates a variable with the given name 
        /// </summary>
        /// <param name="Name">Name of the variable</param>
        public VariablePlain(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// Creates a variable with the given name and value
        /// </summary>
        /// <param name="Name">Name of the variable</param>
        /// <param name="Value">Value of the variable</param>
        public VariablePlain(string Name, Object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        /// <summary>
        /// Creates a read-only variable with the given name and value
        /// </summary>
        /// <param name="Name">Name of the variable</param>
        /// <param name="Value">Value of the variable</param>
        /// <param name="ReadOnly">TRUE of the variable should be read-only</param>
        public VariablePlain(string Name, Object Value, bool ReadOnly)
        {
            this.Name = Name;
            this.Value = Value;
            this.ReadOnly = ReadOnly;
        }

        string _name = "";

        /// <summary>
        /// Name of variable (accessible as $[Name] within PowerShell). The name will be purified to contain only letters, numbers and the underscore character.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {                
                /* MTH: Only allow A-Z,a-z,0-9 and underscore as variable name.
                 * PowerShell documentation: "Whenever possible, variable names should include only alphanumeric characters and the underscore character (_)."
                 * [about_Variables](http://technet.microsoft.com/en-us/library/hh847734.aspx)                
                 */
                string varname = ASCIIStringFilter.Filter(value, ASCIIStringFilter.CHARS_UPPER_CASE + ASCIIStringFilter.CHARS_LOWER_CASE + ASCIIStringFilter.CHARS_NUMERIC + ASCIIStringFilter.CHAR_UNDERSCORE);

                if (string.IsNullOrWhiteSpace(varname))
                    throw new ArgumentException(string.Format("Variable named {0} was empty after purifying it.",value));

                _name=varname;

            }
        }

        /// <summary>
        /// Value of the variable; can be NULL.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// When TRUE trying to write to this variable from a PowerShell script will result in a terminating error
        /// </summary>
        public bool ReadOnly { get; set; }


        public override int GetHashCode()
        {
            /* MTH: I'm confused about GetHashCode(). 
             * [As a result, hash codes (..) should never be used as key fields in a collection (..)"](http://msdn.microsoft.com/en-us/library/system.string.gethashcode%28v=vs.110%29.aspx)
             * But http://stackoverflow.com/questions/15094384/c-sharp-dictionaries-custom-objects says it's OK.
             */
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is VariablePlain)
                return Equals(obj as VariablePlain);
            else
                return false;
        }

        public bool Equals(VariablePlain OtherObject)
        {
            return (this.Name == OtherObject.Name);
        }
    }
}

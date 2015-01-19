using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 *  For the full specification see:
 *  Pearson Education, Inc. - Framework Design Guidelines: Conventions, Idioms, and Patterns for Reusable .NET Libraries, 
 *  2nd Edition by Krzysztof Cwalina and Brad Abrams, 
 *  published Oct 22, 2008 by Addison-Wesley Professional as part of the Microsoft Windows Development Series.
 *
 * http://msdn.microsoft.com/en-us/library/ms229002%28v=vs.110%29.aspx  
 * 
 */
namespace Yamua.Template
{
    //Class names should use PascalCasing
    public abstract class NamingConvention<TObject> where TObject : class, new() //Generic types should start with "T"
    {
        
        //Instance variables should start with "_" and use camelCasing
        int _classLocalVariable = 1; 

        //Function name should use PascalCasing
        void MyFunction(int parameterName) //Parameters should use camelCasing
        {
            int myVariable = 40; //local variables should use camelCasing
            myVariable += _classLocalVariable +1;
        }



    }
}

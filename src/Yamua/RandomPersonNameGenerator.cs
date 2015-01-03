using System;
using System.Collections.Generic;


#region License

//For an explanation see http://www.opensource.org/licenses/bsd-license.php

/*
Copyright (c) 2010, TeX HeX / Xteq Systems 
http://www.texhex.info/  http://www.xteq.com/
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

  * Redistributions of source code must retain the above copyright notice, this 
    list of conditions and the following disclaimer.
  * Redistributions in binary form must reproduce the above copyright notice, 
    this list of conditions and the following disclaimer in the documentation 
    and/or other materials provided with the distribution.
  * Neither the name of the Xteq Systems nor the names of its contributors may 
    be used to endorse or promote products derived from this software without 
    specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE 
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/
#endregion

namespace Yamua
{
    /// <summary>
    /// This class is used to store a name and give easy access to the differnt parts of it.
    /// </summary>
    public class RandomPersonName
    {
        public string First { get; set; } //          Michael
        public string Last { get; set; } //           Hex
        public string Full { get; set; } //           Michael Hex
        public string LastCommaFirst { get; set; }//  Hex, Michael

        public override string ToString()
        {
            return Full;
        }
    }

    /// <summary>
    /// Can be used to easily generate person name like "Michael Taylor", "Lisa Smith" etc.
    /// All names are returned as a RandomPersonName object.
    /// </summary>
    public class RandomPersonNameGenerator
    {
        #region Name arrays
        //All these names (with slight modifications) retrieved from here:
        //
        //http://www.census.gov/genealogy/www/data/1990surnames/
        //
        static readonly string[] _firstNameMaleSet =new string[] {
         "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", 
         "Thomas", "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", 
         "Steven",  "Dirk", "Brian", "Ronald", "Anthony", "Kevin", "Jason", "Matthew", "Gary", 
         "Timothy", "Jose", "Larry", "Jeffrey" 
        };
        static readonly int _firstNameMaleSetCount = _firstNameMaleSet.GetUpperBound(0)+1;

        static readonly string[] _firstNameFemaleSet = new string[] {
            "Mary", "Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", 
            "Margaret", "Melanie", "Lisa", "Nancy", "Karen", "Betty", "Helen", "Sandra", "Donna", 
            "Carol", "Ruth", "Sharon", "Michelle", "Laura", "Sarah", "Kimberly", "Deborah", "Jessica",
            "Shirley", "Cynthia", "Angela", "Melissa"
        };
        static readonly int _firstNameFemaleSetCount = _firstNameFemaleSet.GetUpperBound(0)+1;

        static readonly string[] _lastNameSet = new string[] {
            "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore",
            "Taylor", "Anderson", "Parker", "Jackson", "Norris", "Harris", "Hex", "Thompson", 
            "Garcia", "Martinez", "Robinson", "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall",
            "Allen", "Young", "Hernandez", "King"
        };
        #endregion
        static readonly int _lastNameSetCount = _lastNameSet.GetUpperBound(0)+1;


        RandomPersonName GenerateOne(Random rndm)
        {
            int iRandomValueMale = rndm.Next(0, _firstNameMaleSetCount);
            int iRandomValueFemale = rndm.Next(0, _firstNameFemaleSetCount);
            int iRandomValueLastName = rndm.Next(0, _lastNameSetCount);
            
            RandomPersonName rpn = new RandomPersonName();

            rpn.First = rndm.Next(0, 2) == 0 ? _firstNameMaleSet[iRandomValueMale] : _firstNameFemaleSet[iRandomValueFemale];
            rpn.Last = _lastNameSet[iRandomValueLastName];
            
            rpn.Full = rpn.First + " " + rpn.Last;
            rpn.LastCommaFirst = rpn.Last + ", " + rpn.First;
            
            return rpn;
        }


        /// <summary>
        /// Returns a list of RandomPersonName objects.
        /// </summary>
        /// <param name="Count">Count of names that should be generated</param>
        /// <returns>List of RandomPersonName objects</returns>
        public List<RandomPersonName> Generate(int Count)
        {
            List<RandomPersonName> list = new List<RandomPersonName>();            

            Random rndm = new Random();

            for (int i = 0; i < Count; i++ )
            {
                list.Add(GenerateOne(rndm));
            }

            return list;
        }

        /// <summary>
        /// Returns a list of RandomPersonName objects (Static function)
        /// </summary>
        /// <param name="Count">Count of names that should be generated</param>
        /// <returns>List of RandomPersonName objects</returns>
        public static List<RandomPersonName> StaticGenerate(int Count)
        {
            RandomPersonNameGenerator rpng = new RandomPersonNameGenerator();
            return rpng.Generate(Count);
        }


    }
}

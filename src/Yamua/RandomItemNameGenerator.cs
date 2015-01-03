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
    public class RandomItemName
    {
        public string Prefix { get; set; }   //  Epic
        public string Name { get; set; }     //  Chest
        public string Suffix { get; set; }   //  of Awesomeness
        public string Full { get; set; }     //  Epic Chest of Awesomeness

        public override string ToString()
        {
            return Full;
        }
    }

    public class RandomItemNameGenerator
    {
        #region Name arrays

        static readonly string[] _prefixNameSet = new string[] {
            "Epic", "Heroic", "Normal", "Glowing", "Functional", 
            "Sparkling", "Fluffy", "OMG", "Failing"
        };
        static readonly int _prefixNameSetCount = _prefixNameSet.GetUpperBound(0) + 1;

        static readonly string[] _itemNameSet = new string[] {
            "Chest", "Dynamite", "Chicken", "Orb", "Bacon", "Manual" , 
            "Crowbar", "Anvil", "Fly Swatter", "Rocket", "Scarab"
        };
        static readonly int _itemNameSetCount = _itemNameSet.GetUpperBound(0) + 1;

        static readonly string[] _suffixNameSet = new string[] {
            "of Impending Doom" , "of Function", "of Awesomeness", "Kit", 
            "of CAPSLOCK", "of Failing"
        };
        #endregion
        static readonly int _suffixNameSetCount = _suffixNameSet.GetUpperBound(0) + 1;


        RandomItemName GenerateOne(Random rndm)
        {
            int iRandomValuePrefix = rndm.Next(0, _prefixNameSetCount);
            int iRandomValueName = rndm.Next(0, _itemNameSetCount);
            int iRandomValueSuffix = rndm.Next(0, _suffixNameSetCount);

            RandomItemName rpi = new RandomItemName();

            rpi.Prefix=_prefixNameSet[iRandomValuePrefix];
            rpi.Name=_itemNameSet[iRandomValueName];
            rpi.Suffix=_suffixNameSet[iRandomValueSuffix];

            rpi.Full = rpi.Prefix + " " + rpi.Name + " " + rpi.Suffix;

            return rpi;
        }


        public List<RandomItemName> Generate(int Count)
        {
            List<RandomItemName> list = new List<RandomItemName>();

            Random rndm = new Random();

            for (int i = 0; i < Count; i++)
            {
                list.Add(GenerateOne(rndm));
            }

            return list;
        }

        public static List<RandomItemName> StaticGenerate(int Count)
        {
            RandomItemNameGenerator ring_ring = new RandomItemNameGenerator();
            return ring_ring.Generate(Count);
        }


    }
}

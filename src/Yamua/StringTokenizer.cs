using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Yamua
{
    public class StringTokenizer : IEnumerable
    {
        private string[] elements;

        public StringTokenizer(string source, char[] delimiters)
        {
          elements = source.Split(delimiters);
        }

        public StringTokenizer(string source, char delimiter)
        {
            char[] delimiters ={ delimiter };
            elements = source.Split(delimiters);
        }
        
        // IEnumerable Interface Implementation:
        //   Declaration of the GetEnumerator() method 
        //   required by IEnumerable
        public IEnumerator GetEnumerator()
        {
            return new TokenEnumerator(this);
        }

        public int Length
        {
            get
            {
                return elements.Length;
            }
        }


        // Inner class implements IEnumerator interface:
        private class TokenEnumerator : IEnumerator
        {
            private int position = -1;
            private StringTokenizer t;

            public TokenEnumerator(StringTokenizer t)
            {
                this.t = t;
            }

            // Declare the MoveNext method required by IEnumerator:
            public bool MoveNext()
            {
                if (position < t.elements.Length - 1)
                {
                    position++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // Declare the Reset method required by IEnumerator:
            public void Reset()
            {
                position = -1;
            }

            // Declare the Current property required by IEnumerator:
            public object Current
            {
                get
                {
                    return t.elements[position];
                }
            }
        }


    }
}

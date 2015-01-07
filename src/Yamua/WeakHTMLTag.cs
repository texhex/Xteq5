using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Yamua
{
    
            /*
            //From inner to outer            
            WeakHTMLTag em = new WeakHTMLTag("em");
            em.Text = "My EM Text";

            WeakHTMLTag small = new WeakHTMLTag("small", em);

            WeakHTMLTag td = new WeakHTMLTag("td", small);
            td.CSSClass = "light-green";
            
            htmloutput = td.ToString();
            */
             
            /*
            //From outer to inner
            
            WeakHTMLTag table=new WeakHTMLTag("table");
            table.CSSClass = "dark-grey";
            table.Text = "Table text";

            WeakHTMLTag td = new WeakHTMLTag("td");
            td.CSSClass = "light-green";

            WeakHTMLTag small = new WeakHTMLTag("small");
            small.Attributes["small-attribute"] = "da value";

            WeakHTMLTag em = new WeakHTMLTag("em");
            em.Text = "My EM Text;

            WeakHTMLTag result = WeakHTMLTag.Concat(table, td, small, em);

            htmloutput = result.ToString();
            */

    //XElement performance http://blogs.msdn.com/b/codejunkie/archive/2013/12/09/8992094.aspx
    
    /// <summary>
    /// A minified version to create HTML tags on the fly. Uses XElement from System.Xml.Linq for tags and attributes.
    /// This class is missing a LOT of features. If you need something better, please use HtmlAgilityPack.
    /// .HTML gets the RAW html including everything in angle brackets. When you set it, no encoding is done
    /// .Text assumes normal text and hence when this value is set, the string will be HTML Encoded. 
    /// Attributes you set are ALWAYS HTML encoded. 
    /// </summary>
    public class WeakHTMLTag
    {
        private WeakHTMLTag()
        {
        }

        private const string GUID_REPLACE = "--efbf68de-898d-48ee-b779-556352322259--";

        private string _content = "";

        private XElement _xelem = null;
        private XElement BaseXElement
        {
            get
            {
                return _xelem;
            }
        }

        /// <summary>
        /// Creates an empty HTML tag
        /// </summary>
        /// <param name="TagName">Name of the tag, e.g. td, span, strong etc.</param>
        public WeakHTMLTag(string TagName)
            : this(TagName, WeakHTMLContentType.Text, "")
        {
        }

        /// <summary>
        /// Creates an HTML tag with inner text
        /// </summary>
        /// <param name="TagName">Name of the tag</param>
        /// <param name="Text">Value this tag encloses. Will automatically be HTML encoded</param>
        public WeakHTMLTag(string TagName, string Text)
            : this(TagName, WeakHTMLContentType.Text, Text)
        {
        }

        /// <summary>
        /// Creates an HTML tag with text or HTML set
        /// </summary>
        /// <param name="TagName">Name of the tag</param>
        /// <param name="Format">Type of string found in DATA. HTML or Text</param>
        /// <param name="Data">Data to be set, either as HTML or Text</param>
        public WeakHTMLTag(string TagName, WeakHTMLContentType Format, string Data)
        {
            //XHTML tags should be lowercase
            _xelem = new XElement(TagName.ToLower());
            _xelem.Value = GUID_REPLACE;

            if (Format == WeakHTMLContentType.Text)
            {
                this.Text = Data;
            }
            else
            {
                this.HTML = Data;
            }

            this.Attributes = new WeakHTMLAttributes(_xelem);
        }


        /// <summary>
        /// Creates a new HTML tag that encloses another HTML tag 
        /// </summary>
        /// <param name="TagName">Name of this tag</param>
        /// <param name="EnclosedTag">Tag that will enclodes by this tag. Means: <THISTAG><ENCLOSEDTAG></ENCLOSEDTAG></THISTAG>  </param>
        public WeakHTMLTag(string TagName, WeakHTMLTag EnclosedTag)
            : this(TagName, WeakHTMLContentType.HTML, EnclosedTag.ToString())
        {
            if (EnclosedTag == null)
                throw new ArgumentNullException();

        }

        /// <summary>
        /// Clones an existing HTML tag
        /// </summary>
        /// <param name="CloneSource">The source WeakHTMLTag that should be cloned</param>
        public WeakHTMLTag(WeakHTMLTag CloneSource)
        {
            if (CloneSource == null)
                throw new ArgumentNullException();

            XElement clone = XElement.Parse(CloneSource.BaseXElement.ToString());
            _xelem = clone; //Value is already set to GUID_REPLACE

            this.HTML = CloneSource.HTML;
            this.Attributes = new WeakHTMLAttributes(_xelem);
        }


        /// <summary>
        /// Creates a clone of a existing WeakHTMLTag and encloses another WeakHTMLTag into it.
        /// </summary>
        /// <param name="OuterTag">The WeakHTMLTag that used as the outer tag</param>
        /// <param name="EnclosedTag">The WeakHTMLTag used as the inner (enclosed) tag</param>
        public WeakHTMLTag(WeakHTMLTag OuterTag, WeakHTMLTag EnclosedTag)
            : this(OuterTag)
        {
            if (EnclosedTag == null)
                throw new ArgumentNullException();

            //This element is now correctly set to the data in OuterTag
            //If this HTML tag does not have any value, we can use directly EnclosedTag as data
            if (string.IsNullOrWhiteSpace(this.HTML))
            {
                this.HTML = EnclosedTag.ToString();
            }
            else
            {
                //This HTML tag has a value set, so combine those two
                this.HTML += EnclosedTag.ToString();
            }
 
        }


        /// <summary>
        /// Index properties for the attributes of this HTML tag
        /// </summary>
        public WeakHTMLAttributes Attributes { get; private set; }

        /// <summary>
        /// Text that is enclosed by this HTML tag. Any value going in our out will be HTML encoded. 
        /// </summary>
        public string Text
        {
            get
            {
                return WebUtility.HtmlDecode(_content);
            }
            set
            {
                _content = WebUtility.HtmlEncode(value);
            }
        }


        /// <summary>
        /// Returns the raw content of this tag. No HTML Encoding is done.
        /// </summary>
        public string HTML
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
            }
        }

        public string CSSClass
        {
            get
            {
                return Attributes["class"];
            }

            set
            {
                Attributes["class"] = value;
            }
        }


        /// <summary>
        /// Returns the HMTL output of this tag
        /// </summary>
        /// <returns>This tag in HTML syntax</returns>
        public override string ToString()
        {
            if (_xelem == null)
            {
                throw new InvalidOperationException("Base XElement is null");
            }
            else
            {
                StringBuilder sb = new StringBuilder(_xelem.ToString());

                //Replace the content with our internal value
                sb.Replace(GUID_REPLACE, _content);

                return sb.ToString();
            }
        }


        /// <summary>
        /// Concats several WeakHTMLTags into one. The first pameter is the outer tag, second tag is enclosed in first tag, third tag is enclosed in second tag and so on. 
        /// </summary>
        /// <param name="Tags"></param>
        /// <returns></returns>
        public static WeakHTMLTag Concat(params WeakHTMLTag[] Tags)
        {
            //If we do not have any tag, return an exception
            if (Tags.Length == 0)
            {
                throw new ArgumentException("At least one WeakHTMLTag to concat is required");
            }
            else
            {
                //If we only get one item, return it
                if (Tags.Length == 1)
                {
                    return Tags[0];
                }
                else
                {
                    WeakHTMLTag current = null;
                 
                    //Loop all objects, starting with the last
                    for (int i = Tags.Length - 1; i >= 0; i--)
                    {
                        if (current != null)
                        {
                            WeakHTMLTag temp = new WeakHTMLTag(Tags[i], current);
                            current = temp;
                        }
                        else
                        {
                            //No current is present so far, use this element
                            current = Tags[i];
                        }
                    }

                    return current;
                }
            }

        }

        /// <summary>
        /// Encode a string to HTML (< becoms &gt; etc.)
        /// </summary>
        /// <param name="Data">Non HTML encoded string</param>
        /// <returns>HTML encoded string</returns>
        public static string HTMLEncode(string Data)
        {
            return WebUtility.HtmlEncode(Data);
        }

        /// <summary>
        /// Returns a decoded HTML string
        /// </summary>
        /// <param name="Data">HTML encoded string</param>
        /// <returns>Raw text</returns>
        public static string HTMLDecode(string Data)
        {
            return WebUtility.HtmlDecode(Data);
        }
         
    }



    //Internal class to allow indexed property Attributes
    public class WeakHTMLAttributes
    {
        private WeakHTMLAttributes()
        {
        }

        XElement _element;

        internal WeakHTMLAttributes(XElement BaseElement)
        {
            _element = BaseElement;
        }

        public string this[string Name]
        {
            get
            {
                if (_element.Attribute(Name) != null)
                {
                    return _element.Attribute(Name).Value;
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                string val = WebUtility.HtmlEncode(value);
                _element.SetAttributeValue(Name, val);
            }
        }

    }


    //Enum for WeakHTMLTag
    public enum WeakHTMLContentType
    {
        HTML = 1,
        Text = 2
    }
}

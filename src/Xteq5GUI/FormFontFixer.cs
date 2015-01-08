using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xteq5GUI
{
    //Original idea and code (3 lines :-) by Benjamin Hollis: http://brh.numbera.com/blog/index.php/2007/04/11/setting-the-correct-default-font-in-net-windows-forms-apps/
    //Copyright (C) TeX HeX of Xteq Systems: http://texhex.blogspot.com/ and http://www.texhex.info/
    public static class FormFontFixer
    {
        //This list contains the fonts we want to replace.
        static readonly List<string> FontReplaceList
                 = new List<string>(new string[] { "Microsoft Sans Serif", "Tahoma" });


        static Font _DefaultFont;
        static bool _CanFixFonts;


        static FormFontFixer()
        {
            //Basically the font name we want to use should be easy to choose by using the SystemFonts class. However, this class
            //is hard-coded (!!) and doesn't seem to work right. On XP, it will mostly return "Microsoft Sans Serif" except
            //for the DialogFont property (=Tahoma) but on Vista, this class will return "Tahoma" instead of "SegoiUI" for this property!

            //Therefore we will do the following: If we are running on a OS below XP, we will exit because the only font available
            //will be MS Sans Serif. On XP, we gonna use "Tahoma", and any other OS we will use the value of the MessageBoxFont
            //property because this seems to be set correctly on Vista an above.

            if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
            {
                //95, 98 and other crap
                _CanFixFonts = false;
                return;
            }

            if (Environment.OSVersion.Version.Major < 5)
            {
                //Windows NT
                _CanFixFonts = false;
                return;
            }

            if (Environment.OSVersion.Version.Major < 6)
            {
                //Windows 2000 (5.0), Windows XP (5.1), Windows Server 2003 and XP Pro x64 Edtion v2003 (5.2)
                _CanFixFonts = true;
                _DefaultFont = SystemFonts.DialogFont; //Tahoma hopefully
            }
            else
            {
                //Vista and above
                _CanFixFonts = true;
                _DefaultFont = SystemFonts.MessageBoxFont; //should be SegoiUI
            }
        }

        public static void Fix(Form form)
        {
            //If we can't fix the font, exit
            if (_CanFixFonts == false)
            {
                return;
            }


            //Now start with the real work...
            foreach (Control c in form.Controls)
            {
                //only replace fonts that use one the "system fonts" we have declared
                if (FontReplaceList.IndexOf(c.Font.Name) > -1)
                {
                    //Now check the size, when the size is 9 or below it's the default font size and we do not keep the size since
                    //SegoiUI has a complete different spacing (and thus size) than MS SansS or Tahoma.

                    //Also check if there are any styles applied on the font (e.g. Italic) which we need to apply to the new
                    //font as well.

                    bool bUseDefaultSize = true;
                    bool bUseDefaultStyle = true;

                    //is this a special size?
                    if ((c.Font.Size <= 8) || (c.Font.Size >= 9))
                    {
                        bUseDefaultSize = false;
                    }

                    //are any special styles (bold, italic etc.) applied to this font?
                    if ((c.Font.Italic == true) ||
                         (c.Font.Strikeout == true) ||
                         (c.Font.Underline == true) ||
                         (c.Font.Bold == true))
                    {
                        bUseDefaultStyle = false;
                    }

                    //if everything is set to defaults, we can use our prepared font right away
                    if ((bUseDefaultSize == true) && (bUseDefaultStyle == true))
                    {
                        c.Font = _DefaultFont;
                    }
                    else
                    {
                        //There are non default properties set so
                        //there is some work we need to do...


                        //Restrive custom font style
                        FontStyle Style = FontStyle.Regular;
                        if (bUseDefaultStyle == false)
                        {
                            if (c.Font.Italic)
                            {
                                Style = Style | FontStyle.Italic;
                            }
                            if (c.Font.Strikeout)
                            {
                                Style = Style | FontStyle.Strikeout;
                            }
                            if (c.Font.Underline)
                            {
                                Style = Style | FontStyle.Underline;
                            }
                            if (c.Font.Bold)
                            {
                                Style = Style | FontStyle.Bold;
                            }
                        }

                        //Retrive custom size
                        float fFontSize = _DefaultFont.SizeInPoints;
                        if (bUseDefaultSize == false)
                        {
                            fFontSize = c.Font.SizeInPoints;

                        }

                        //Finally apply this font...
                        Font font = new Font(_DefaultFont.Name, fFontSize, Style, GraphicsUnit.Point);
                        c.Font = font;

                    }
                }
            }

        }
    }
}

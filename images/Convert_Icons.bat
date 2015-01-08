REM Alternatives
REM  http://www.icoconverter.com/
REM  http://convertico.org/image_to_icon_converter/

REM Best solution
REM  http://www.axialis.com/download/iw.html

REM Convert using http://www.imagemagick.org/

SET EXE_CONVERT=C:\_Tools\ImageMagick-6.9.0-2\convert.exe
REM -colors 256 
"%EXE_CONVERT%" Xteq5_Icon_16x16.ico Xteq5_Icon_32x32.ico Xteq5_Icon_48x48.ico Xteq5_Icon.ico

pause

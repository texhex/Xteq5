<img src="https://github.com/texhex/Xteq5/raw/master/images/Xteq5_NameLogo_Small.png" alt="Xteq5 logo" title="Xteq5" style="max-width:100%;" />

<!--
About Xteq5
===========
<img src="https://github.com/texhex/Xteq5/raw/master/images/Xteq5_Small.png" alt="Xteq5 logo" title="Xteq5" align="right" style="max-width:100%;" />
-->
 
A known issues testing framework, powered by PowerShell scripts.

Why you want to use it: Computer systems today are complex. Some years ago, if software A did not run, the reason was “A requires B and B is not installed”. Today it’s more like “A does not run because B is not working as the value C in D is not set by E”. 

Using TestUtil, you could now write PowerShell scripts that test A, B, C, D and E. The generated HTML report ([Example][_examplereport]) will then inform you if, and what, isn’t right. From that time on, you don’t have to remember this case anymore. Just run TestUtil again and you will be informed in seconds.

This procedure can lead to cost savings if you have separated 1st and 2nd level support teams. The 1st level support can check with TestUtil if the problem is already known and fix it without escalating to 2nd level. This means higher first call resolution rates for 1st level and less tickets for 2nd level. 

<!-- I THINK THIS IS NO LONGER TRUE

**EARLY RELEASE WARNING:** TestUtil is still under heavy development. Running scripts will work fine, but it does not include a lot of them out of the box. It's nearly useless at this time for "normal" home users, but ready for administrators that plan to utilize it for custom scripts. 
-->


##<a name="download">Download</a>

The most recent version can be downloaded from [Releases][_download].


##<a name="sysrequirements">System Requirements</a>

<!-- These links are also used in /src/setup/_Setup.iss -->

* Windows 7 SP1 / Windows Server 2008 SP2 (or higher)
* 256MB free memory, ~5 MB free disk space
* [.NET Framework 4.5][_netframework] (or higher)
* [PowerShell 4.0][_wmf] (or higher)
* Download links are provided by Setup.exe if one of the components is not installed


##<a name="docs">Documentation</a>

Please see the [Xteq5 Wiki][_wiki]

##<a name="contribute">Contributions</a>

<!-- https://help.github.com/articles/using-pull-requests/ -->

Constructive contributions are very welcome! 

If you encounter a bug, please create a new [issue][_issuenew], describing how to reproduce the bug and we will try to fix it. 

If you have created a script you want us to include, please open a new [issue][_issuenew] (with the label _addscript_) and include a download link to your script. Please make sure that the script implements the recommendations described in the [Wiki][_wiki_newscript].


##<a name="copyright">Copyright and License</a>

Copyright © 2010-2015 [Michael Hex][_texhexhomepage] ([@texhex][_texhexgithub]).

Licensed under the **Apache License, Version 2.0**. For details, please see [LICENSE.txt][_license].

*Xteq5* and the *Radar* logo (Created by [designklima](http://designklima.com/)) - Copyright © 2010-2015 Michael Hex - All Rights Reserved 




[_logo]:images/testutil_small.png
[_license]:licenses/LICENSE.txt
[_wiki]:https://github.com/texhex/xteq5/wiki
[_download]:https://github.com/texhex/Xteq5/releases
[_netframework]:http://www.microsoft.com/en-us/download/details.aspx?id=40773
[_wmf]:http://www.microsoft.com/en-us/download/details.aspx?id=40855
[_issuenew]:https://github.com/texhex/Xteq5/issues/new
[_wiki_newscript]:https://github.com/texhex/xteq5/wiki/_fwLinkScript
[_texhexgithub]:https://github.com/texhex/
[_texhexhomepage]:http://www.texhex.info/
[_examplereport]:http://texhex.github.io/xteq5/examplereport.html


TestUtil
========

<img src="https://github.com/texhex/testutil/raw/master/images/testutil_small.png" alt="TestUtil logo" title="TestUtil" align="right" style="max-width:100%;" />
 
A known issues testing framework, powered by PowerShell scripts.

Why you want to use it: Computer systems today are complex. Some years ago, if software A did not run, the reason was “A requires B and B is not installed”. Today it’s more like “A does not run because B is not working as the value C in D is not set to E which should have been done by F”. 

Using TestUtil, you could now write PowerShell script that test A, B, C, D and E. The generated HTML report will then inform you if, and what, isn’t right. From that time on, you don’t have to remember this case anymore. If another system has a problem, just run TestUtil again and you will be informed in seconds.

**EARLY RELEASE WARNING:** TestUtil is still under heavy development. Running scripts will work fine, but it does not include a lot of them out of the box. It's nearly useless at this time for "normal" home users, but ready for administrators that plan to utilize it for custom scripts. 


##<a name="download">Download</a>

The most recent version can be downloaded from [Releases][_download].


##<a name="sysrequirements">System Requirements</a>

<!-- These links are also used in /src/setup/_TestUtil.iss -->

* Windows 7 SP1 / Windows Server 2008 SP2 (or higher)
* 256MB free memory, ~5 MB free disk space
* [.NET Framework 4.5][_netfw] (or higher)
* [PowerShell 4.0][_wmf] (or higher)
* Download links are provided by Setup.exe if one of these components is not installed


##<a name="docs">Documentation</a>

Please see the [TestUtil Wiki][_wiki]

##<a name="contribute">Contributions</a>

<!-- https://help.github.com/articles/using-pull-requests/ -->

Constructive contributions are very welcome! 

If you encounter a bug, please create a new [issue][_issuenew], describing how to reproduce the bug and we will try to fix it. 

If you have created a script you want us to include, please open a new [issue][_issuenew] (with the label _addscript_) and include a download link to your script. Please make sure that the script implements the recommendations described in the [Wiki][_wiki_newscript].


##<a name="copyright">Copyright and License</a>

Copyright © 2010-2015 [Michael 'Tex' Hex][_texhexhomepage] ([@texhex][_texhexgithub]), licensed under the **Apache License, Version 2.0**.

For details, please see [LICENSE.txt][_license].

*TestUtil* and the *Radar* icon/logo - Copyright © 2010-2015 Michael 'Tex' Hex - All Rights Reserved - Created by [designklima](http://designklima.com/)




[_logo]:images/testutil_small.png
[_license]:licenses/LICENSE.txt
[_wiki]:https://github.com/texhex/testutil/wiki
[_download]:https://github.com/texhex/testutil/releases
[_netfw]:http://www.microsoft.com/en-us/download/details.aspx?id=40773
[_wmf]:http://www.microsoft.com/en-us/download/details.aspx?id=40855
[_issuenew]:https://github.com/texhex/testutil/issues/new
[_wiki_newscript]:https://github.com/texhex/testutil/wiki/_fwLinkScript
[_texhexgithub]:https://github.com/texhex/
[_texhexhomepage]:http://www.texhex.info/

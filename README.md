<img src="https://github.com/texhex/Xteq5/raw/master/images/Xteq5_NameLogo3.png" alt="Xteq5 logo" title="Xteq5" style="max-width:100%;" />

<!--
About Xteq5
===========
<img src="https://github.com/texhex/Xteq5/raw/master/images/Xteq5_Small.png" alt="Xteq5 logo" title="Xteq5" align="right" style="max-width:100%;" />
-->
 
A known issues testing framework, powered by PowerShell scripts.

Computer systems today are complex. Some years ago, if software A did not run, the reason was "A requires B and B is not installed". Today it’s more like "A does not run because B is not working as the value C in D is not set by E". 

Using Xteq5, you could now write PowerShell scripts that test A, B, C, D and E. The generated HTML report ([Example][_examplereport]) will then inform you if, and what, isn’t right. From that time on, you don’t have to remember this case anymore. Just run Xteq5 again and you will be informed in seconds. This can also be done silently using the command line, resulting in a XML or JSON report for further automated processing. 

This procedure can lead to cost savings if you have separated 1st and 2nd level support teams. The 1st level support can check with Xteq5 if the problem is already known and fix it without escalating to 2nd level. This means higher first call resolution rates for 1st level and less tickets for 2nd level. 

<!-- I THINK THIS IS NO LONGER TRUE

**EARLY RELEASE WARNING:** Xteq5 is still under heavy development. Running scripts will work fine, but it does not include a lot of them out of the box. It's nearly useless at this time for "normal" home users, but ready for administrators that plan to utilize it for custom scripts. 
-->


##<a name="download">Download</a>

The most recent version can be downloaded from [Releases][_downloads].


##<a name="sysrequirements">System Requirements</a>

<!-- These links are also used in /src/setup/_Setup.iss -->

* Windows 7 SP1 / Windows Server 2008 SP2 (or higher)
* 256MB free memory, 6 MB free disk space
* [.NET Framework 4.5][_netframework] (or higher)
* [PowerShell 4.0][_wmf] (or higher)
* Download links are provided by Setup.exe if one of the components is not installed


##<a name="docs">Documentation</a>

Please see the [Xteq5 Wiki][_wiki] for documentation, including [How to use][_wiki_howto], creating your [own assets or tests][_wiki_customscripts], [customize][_wiki_setup] the installation or utilizing the [command line interface][_wiki_cli].  

##<a name="contribute">Contributions</a>

Any constructive contribution is very welcome! 

If you encounter a bug, please create a new [issue][_issuenew], describing how to reproduce the bug and we will try to fix it. 

In case you have created a script you want us to include, please open a new [issue][_issuenew] (with the label _addscript_) and include a download link to your script. Please make sure that the script implements the recommendations described in the [Wiki][_wiki_newscript].

Pull requests for source code changes or scripts are welcome as well.  


##<a name="copyright">Copyright and License</a>

Copyright © 2010-2015 [Michael Hex][_texhexhomepage] ([@texhex][_texhexgithub]).

Licensed under the **Apache License, Version 2.0**. For details, please see [LICENSE.txt][_license].

*Xteq5* and the *Radar* logo (Created by [designklima](http://designklima.com/)) - Copyright © 2010-2015 Michael Hex - All Rights Reserved 




[_logo]:images/Xteq5_small.png
[_netframework]:http://www.microsoft.com/en-us/download/details.aspx?id=40773
[_wmf]:http://www.microsoft.com/en-us/download/details.aspx?id=40855


[_issuenew]:https://github.com/texhex/Xteq5/issues/new
[_wiki_newscript]:https://github.com/texhex/xteq5/wiki/_fwLinkScript


<!-- List of links from Home.md-->
[_readme_requirements]:https://github.com/texhex/Xteq5/blob/master/README.md#system-requirements
[_readme_contribute]:https://github.com/texhex/Xteq5/blob/master/README.md#contributions
[_downloads]: https://github.com/texhex/Xteq5/releases
[_license]: https://github.com/texhex/Xteq5/blob/master/licenses/LICENSE.txt
[_projecthome]: https://github.com/texhex/Xteq5/
[_example1ise]:https://raw.githubusercontent.com/texhex/Xteq5/master/images/example1_ise.png
[_example1report]:https://raw.githubusercontent.com/texhex/Xteq5/master/images/example1_report.png
[_texhexgithub]:https://github.com/texhex/
[_texhexhomepage]:http://www.texhex.info/
[_examplereport]:http://texhex.github.io/Xteq5/examplereport.html
[_wiki]: https://github.com/texhex/xteq5/wiki
[_wiki_howto]: https://github.com/texhex/xteq5/wiki/How-to-use
[_wiki_customscripts]: https://github.com/texhex/xteq5/wiki/Custom-scripts
[_wiki_setup]: https://github.com/texhex/xteq5/wiki/Setup-customization
[_wiki_cli]: https://github.com/texhex/xteq5/wiki/Command-line-interface


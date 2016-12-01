## Setting up and starting Cuis using a Windows or MacOS (no command line required)

What follows are instructions for setting up Cuis on Windows or Mac OS X without using the command line. If you are familiar with Git, please see [Getting started using Git Bash](GettingStarted-UsingGitAndCommandline.md) . This document is intended for beginners, and tries to require as little previous knowledge as possible.

If you want to contribute back to the community, you might subscribe to the Cuis mail list at http://http://cuis-smalltalk.org/mailman/listinfo/cuis-dev_cuis-smalltalk.org , and email your code there.

The Cuis image specified is the most current one. The VM specified is the latest one too.

### On Windows ###
* Crea a new folder in your machine using Windows Explorer
* download [`master.zip`](https://github.com/Cuis-Smalltalk/Cuis-Smalltalk-Dev/archive/master.zip)
* extract the contents of the zip file to your folder
* download [`cog_win32x86_squeak.cog.spur_201608171728.zip`](https://github.com/OpenSmalltalk/opensmalltalk-vm/releases/download/201608171728/cog_win32x86_squeak.cog.spur_201608171728.zip)
* extract the contents of the zip file to your folder
* drop the Cuis5.0-2974-spur.image over the Squeak.exe file

### On Mac OS X ###
* Crea a new folder in your machine using Mac Finder
* download [`master.zip`](https://github.com/Cuis-Smalltalk/Cuis-Smalltalk-Dev/archive/master.zip)
* extract the contents of the zip file to your folder
* download [`cog_macos32x86_squeak.cog.spur_201608171728.tar.gz`](https://github.com/OpenSmalltalk/opensmalltalk-vm/releases/download/201608171728/cog_macos32x86_squeak.cog.spur_201608171728.tar.gz)
* extract the contents of the .tar.gz file to your folder
* drop the Cuis5.0-2974-spur.image over the Squeak.app file
* [ctrl] click on Cog application. Open. "Cog is from an unidentified developer"; dialog. Click on [Open] Supply required credentials. (This is required only on the first run). Select Cuis5.0-2974-spur.image. (If this step is a problem, you might try the command line variant, described in another document.)
* On subsequent runs, drop the Cuis5.0-2974-spur.image over the Squeak.app file

### Notes ###
* If you can't find Cuis5.0-2974-spur.image, then this document is outdated. Use the Cuis spur image with the latest update number available.
* If you can't find the Squeak Cog Spur VM specified, then this document is outdated. Use the the Squeak Cog Spur VM for your platform with the latest Date and Time available.

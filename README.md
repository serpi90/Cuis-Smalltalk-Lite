# Cuis

[Cuis](http://www.cuis-smalltalk.org) is a free [Smalltalk-80](https://en.wikipedia.org/wiki/Smalltalk) environment originally derived from [Squeak](http://www.squeak.org) with a specific set of goals: being simple and powerful. It is also portable to any platform, fast and efficient. This means it is a great tool for running on any hardware, ranging from RasPis and the like and phones, up to server racks, and everything in between, including regular laptops and PCs.

Cuis is
* Small
* Clean
* Appropriable

Like Squeak, Cuis is also:
* Open Source
* Self contained
* Multiplatform

Like other Smalltalk-80 environments (including Squeak, Pharo and others), Cuis is also:
* A complete development environment written in itself
* A pure, dynamic Object Oriented language

What sets Cuis appart from the rest of the Squeak family is that it takes an active attitude towards system complexity:

Unbound complexity growth, together with develpment strategies focused only in the short term, are the worst long term enemies of all software systems. As systems grow older, they usually become more complex. New features are added as layers on top of whatever is below, sometimes without really understanding it, and almost always without modifying it. Complexity and size grow without control. Evolution slows down. Understanding the system becomes harder every day. Bugs are harder to fix. Codebases become huge for no clear reason. At some point, the system can't evolve anymore and becomes "legacy code".

Complexity puts a limit to the level of understanding of the system a person might reach, and therefore limits the things that can be done with it. Dan Ingalls says all this in ["Design Principles Behind Smalltalk"](http://www.cs.virginia.edu/~evans/cs655/readings/smalltalk.html). Even if you have already done so, please go and read it again!

This presentation by Rich Hickey, ["Simple made Easy"](http://www.infoq.com/presentations/Simple-Made-Easy) is also an excellent reflection on these values.

We follow a set of ideas that started with Jean Piaget's ["Constructivism"](https://en.wikipedia.org/wiki/Constructivism_(philosophy_of_education)), and later explored in Seymour Papert's ["Mathland"](https://en.wikipedia.org/wiki/Experiential_learning). These lead to Alan Kay's Learning Research Group's ["Dynabook"](http://www.vpri.org/pdf/hc_pers_comp_for_children.pdf) ["also"](http://www.vpri.org/pdf/hc_what_Is_a_dynabook.pdf) ["and"](http://www.vpri.org/pdf/m1977001_dynamedia.pdf) and to [Smalltalk-80](https://en.wikipedia.org/wiki/Smalltalk). To us, a Smalltalk system is a Dynabook. A place to experiment and learn, and a medium to express the knlowledge we acquire. We understand software development as the activity of learning and documenting knowledge, for us and others to use, and also to be run on a computer. The fact that the computer run is useful is a consequence of the knowldege being sound and relevant. (Making it run is _not_ the important part!)

Cuis Smalltalk is our attempt at this. Furthermore, we believe we are doing something else that no other Smalltalk, commercial or open source, does. We attempt to give the true Smalltalk-80 experience, and keep Smalltalk-80 not as legacy software to be run in an emulator, but as a live, evolving system. We feel we are the real keepers of Smalltalk-80, and enablers of the Dynabook experience.

Cuis is continuously evolving towards simplicity. Each release is better (i.e. simpler) than the previous one. At the same time, features are enhanced, and any reported bugs fixed. We also adopt recent enhancements from Squeak.

### Setting up Cuis in your machine ###
If you are not familiar with Git, follow the instructions in [Getting started using GUI](Documentation/GettingStarted-UsingGUI.md) or [Getting started using commandline](Documentation/GettingStarted-UsingCommandline.md) .

If you are familiar with Git, it might be best to follow [Getting started using Git Bash](Documentation/GettingStarted-UsingGitAndCommandline.md) .

### Getting Started ###
If you are learning Smalltalk, the Cuis community can help you. Check the ["Learning Cuis Smalltalk"](https://github.com/Cuis-Smalltalk-Learning/Learning-Cuis "Learning Cuis Smalltalk") repository. It includes several great tutorials. Also, the TerseGuide.pck.st package (in the /Packages folder in this repo) is useful both as a guide and a reference.

Additionally, there are many tutorials and references for Smalltalk in the web. They apply quite well to Cuis, especially those written originally for Smalltalk-80 or Squeak. These books ["Smalltalk-80 the language and its implementation"](http://stephane.ducasse.free.fr/FreeBooks/BlueBook/Bluebook.pdf) and ["Inside Smalltalk volume I"](http://stephane.ducasse.free.fr/FreeBooks/InsideST/InsideSmalltalk.pdf) are great introductory texts, and they are also the reference for the language and basic class library. Both are freely available.

The user interface enables you to access most of the code and conduct Smalltalk experiments on your own. You can review its features at ["Quick Tour of the UI"](https://github.com/Cuis-Smalltalk-Learning/Learning-Cuis/blob/master/Quick-UI-Tour.md). 

### Contributing to Cuis ###
Cuis is maintained on https://github.com/Cuis-Smalltalk.

Please read [Code Management in Cuis](Documentation/CodeManagementInCuis.md), about developing packages for Cuis, and [Using Git and GitHub to host and manage Cuis code](Documentation/CuisAndGitHub.md). While Cuis should work equally well with any file-based DVCS, we encourage the use of Git and GitHub.

In any case, we also accept contributions as ChangeSet files in email. Any contribution must be under the MIT license.

To contribute code, please use an image with all included packages already loaded, using updated versions, especially, of any affected packages. This will ensure we don't break them while we evolve Cuis.
Here is a Smalltalk script to load all packages currently included:
```
Feature require: 'Core-Packages'
```

Cuis is distributed subject to the MIT License. See the LICENSE file. Any contribution submitted for incorporation into or for distribution with Cuis shall be presumed subject to the same license.

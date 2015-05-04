Interface Booster: Provider Plugin API
===================================

This repository contains the source code for the Interface Booster Visual Studio Extension. This Extension can be used to create new Interface Booster Plugin Projects.

Building this projects results in a .vsix Microsoft Visual Studio Extension file.

## What is a Visual Studio Extension?

See: [Products and Extensions for Visual Studio](https://visualstudiogallery.msdn.microsoft.com/)

## What do I need to work on the Extension?

* Microsoft .NET Framework 4.5
* A development environment (e.g. MSBuild, Visual Studio)
* [Microsoft Visual Studio 2012 SDK](https://www.microsoft.com/en-us/download/details.aspx?id=30668)

## Content

Directory | Description
----------| -------------
/build | files used for building and deploying the project
/src | the source code (Visual Studio Solution and Projects)
/src/InterfaceBooster.ProviderPluginTemplate | Template Project (not meant for building because of the use of [Template Parameters](https://msdn.microsoft.com/en-us/library/eehb4faa.aspx))
/src/InterfaceBooster.VisualStudioExtension | Visual Studio Extension Projects (creates a .vsix file)
/tools | scripts and software used for development

## License

This project is licensed under the terms of the GNU General Public License (GPL) Version 3. See LICENSE for more information.

## Contact

Workbooster GmbH<br/>
Obermuelistrasse 85<br/>
8320 Fehraltorf (Switzerland)<br/>

Web: www.workbooster.ch<br/>
E-Mail: info@workbooster.ch<br/>
Phone: +41 (0)44 515 48 80<br/>
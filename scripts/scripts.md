
# Directory scripts/

[IGLibGraphics3D repository](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/) / [scripts/](#directory-scripts) ([repo](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/blob/main/scripts/ScriptsDirectory.md) / [repo dir](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/tree/main/scripts))

This directory contains **scripts** that support **development, building, deployment**, etc., of the [current repository](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/).

**Contents**:
* [Repository's scripts](#repositorys-scripts)

## Repository's Scripts

* *[UpdateOrCloneRepository.ps1](./UpdateOrCloneRepository.ps1)* is a generic cloning / updating script copied from the [IGLibScripts repository](https://github.com/ajgorhoe/IGLib.modules.IGLibScripts/).
* *[UpdateOrCloneRepository.ps1](./UpdateOrCloneRepository.ps1)* clones (if they are not cloed yet) or updates all repositories of dependencies that are used via source code projects rather than via NuGet packages.
* *[UpdateDepencencyReposExtended.ps1](./UpdateDepencencyReposExtended.ps1)* updates or restores all  dependencies that are updated by the previous script, plus eventually some additional repositories that may be used conditionally or just for browsing.
* Scripts *`[UpdateRepo_*.ps1]()`* update or clone individual repositories.

In order to run  the scripts, you need the Windows Powershell installed (on Windows) or the open source cross-platform PowerShell (pwsh.exe) installed (all systems).

These scripts clone or update the necessary dependency repositories into correct locations relative to this script directory. These locations are usually outsode of the clone directory of the current repository, most often in ../../ relative to the script directory (i.e., in the directory that contains the current repository clone; this is usually the case for other IGLib repositories) or in ../../_external/ (this is usually true for dependencies that are not part of IGLib).
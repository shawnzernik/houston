# Houston Container Playground

The following is an exploration project of using docker and k8s.  The outline below explains the structure and files.

- Batch Files -- these are here to document the Docker commands used.
  - create-network.bat & rm-network.bat -- create a "houston" network that all the docker containers will run in.  This will allow the docker containers to talk together, but be isolated from the rest of the world.  You'll need to expose container ports as needed.
  - create, start, stop, and rm containers -- these create the individual containers.
  - **these have been moved to '.vscode\tasks.json'**
- MessageBoard-DO NOT USE -- this is a boilerplate .NET WebAPI project in a docker container.
- htown-http -- this is a simple web server with a index.html copied into it to test creating custom docker containers
- htown-msg -- this is a message board web application in Visual Studio (not vscode).  This uses a database (mssql), web apis, and a simple html front end.  This will be the basis for what I'm looking to containerize and break apart.  There is a DB project that can be deployed to MS SQL to create your database.
- htown-mssql -- this is the mssql server used by htown-msg
  - *the log and data files are being stored on the local filesystem*
- htown-ubuntu -- this is an ubuntu box on the houston network to troubleshoot with
  - *the root users home directory is stored on the local filesystem*

## Platform Independence

.NET Core, not Framework, is platform independent.  Running Visual Studio on a MAC will EOL on 4/2024.  As a result, this code base is moving everything to run in VS Code natively.  Below are the software dependencies that'll need installed.

### Commands Used

To see the docker commands, please reference:

- .vscode\tasks.json

### Dependencies Required

- VS Code Extensions
  - .NET Install Tool - <https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.vscode-dotnet-runtime>
  - C# -  <https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp>
  - C# Dev Kit - <https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit>
  - Code Spell Checker - <https://marketplace.visualstudio.com/items?itemName=streetsidesoftware.code-spell-checker>
  - Dev Containers - <https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers>
  - Docker - <https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker>
  - Draw.io Integration - <https://marketplace.visualstudio.com/items?itemName=hediet.vscode-drawio>
  - ESLint - <https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint>
  - IntelliCode for C# Dev Kit - <https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.vscodeintellicode-csharp>
  - markdownlint - <https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint>
  - SQL Database Projects (DID NOT INSTALL) - <https://marketplace.visualstudio.com/items?itemName=ms-mssql.sql-database-projects-vscode>
- .NET 0.8 SDK - <https://dotnet.microsoft.com/en-us/download/dotnet/8.0>
- Installing SQL Package Command - <https://learn.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage-download?view=sql-server-ver16>
- SqlProjects for VS Code - <https://github.com/rr-wfm/MSBuild.Sdk.SqlProj>

### SQL Project / DacFX

The following URL is to the Microsoft GitHub site:

- <https://github.com/microsoft/DacFx>

The following explains how to use it:

- <https://github.com/microsoft/DacFx/tree/main/src/Microsoft.Build.Sql.Templates>

A tutorial is here:

- <https://github.com/microsoft/DacFx/blob/main/src/Microsoft.Build.Sql/docs/Tutorial.md>

Additional Documentation:

- <https://github.com/microsoft/DacFx/blob/main/src/Microsoft.Build.Sql/docs/Converting-Existing.md>
- <https://github.com/microsoft/DacFx/blob/main/src/Microsoft.Build.Sql/docs/Functionality.md>

### Moving to DotNet CLI for Everything

- Rename solution file by postfixing '.vs2022'
- Create new solution file - <https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-sln>
  - dotnet new sln --name "htown-msg"
- Create rename old project file by postfixing '.vs2022'
- Create new project file - <https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new>
  - dotnet new web --framework net8.0 --language "C#" --name "webapi"
  - I did this in a "test" folder and then compared the files to the ones the VS 2022 create.  The big difference was the addition of HTTP in the "launchSettings.json".  Effectively I kept the VS 2022 files.
- Rename ".sqlproj" to ".sqlproj.vs2022"
- Create new "sqlproj" file:
  - dotnet new -i Microsoft.Build.Sql.Templates
  - dotnet new sqlproj --target-platform "SqlAzureV12" --name "database"
- Add projects to solution:
  - dotnet sln htown-msg.sln add webapi
  - dotnet sln htown-msg.sln add database

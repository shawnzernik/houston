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
- HashiCorp Terraform - <https://marketplace.visualstudio.com/items?itemName=HashiCorp.terraform>
- ChatGPT - <https://marketplace.visualstudio.com/items?itemName=zhang-renyang.chat-gpt>

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

### Adding Nuget Dependencies

Use the following to searhc for a Nuget dependency:

- nuget search NAME

Use the following to add a Nuget Dependency:

- dotnet add "webapi.csproj" package Microsoft.Extensions.Logging.Console

## Azure Setup

The resources in Azure are not publically accessible outside Azure.  This poses a problem as we need to interact with the database and other services / servers directly.  To solve this, a OpenVPN server was configured and installed as an EC2 instance.  I used the following subscription:

- OpenVPN Access Server (5 Connected Devices)

Follow the directions provided by the vendor - they do a good job getting everything setup.  The network model I used was NAT.  I also allowed the DNS to be modified on clients.  This allows me to use the Azure host names directly.

### Configuring the CLI

Use the following to "configure" the CLI.  This will allow you to skip the "aws configure" command:

- export AWS_ACCESS_KEY_ID=<USERS_ACCESS_KEY>
- export AWS_SECRET_ACCESS_KEY=<SECRET_FOR_ACCESS_KEY>
- export AWS_DEFAULT_REGION=<REGION>

## Connecting to MS SQL from the Mac

Microsoft SQL Management Studio is a Windows only product.  From the Mac, I used Azure Data Studio.

## Pipeline Simulation -- WORK IN PROGRESS

In the pipeline folder you will find a series of scripts that will deploy the application to AWS.  Below is a description of files and folders:

- terraform - this folder contains the Terraform scripts used.
- kubernetes - this folder has the Kubernetes scripts used.
- build.sh - this build the docker container and pushes it to the AWS container repository
- apply-tf.sh - this provisions the SQL server and Kubernetes cluster.
- apply-k8s.sh - this applies the kubernetes scripts.
- publish-db.sh - this publishes the database

Collectively, you'll need to connect to the VPN and run the scripts from the "pipeline" folder in the following order:

1. build.sh
2. apply-tf.sh
3. publish-db.sh
4. apply-k8s.sh

You should now have a working application in AWS.

## Terraform Logging / Debugging

- <https://developer.hashicorp.com/terraform/internals/debuggingexpo>
- <https://developer.hashicorp.com/terraform/tutorials/configuration-language/troubleshooting-workflow#bug-reporting-best-practices?utm_source=WEBSITE&utm_medium=WEB_IO&utm_offer=ARTICLE_PAGE&utm_content=DOCS>

## Install eksctl

- <https://eksctl.io/installation/>

## Conecting kubectl

Validate AWS CLI connection and user:

- aws sts get-caller-identity

Make a backup of your config file:

- cp ~/.kube/config ~/kube-config-old

Update the config file:

- aws eks update-kubeconfig --region us-east-2 --name houston-eks

View contexts:

- kubectl config view

List current context:

- kubectl config current-context

See all avaiable contexts:

- kubectl config get-contexts

Set current context:

- kubectl config use-context arn:aws:eks:us-east-2:090378945367:cluster/houston-eks
- kubectl config use-context docker-desktop

Test the connection:

- kubectl get svc

## Amazon EKS Addons

Use the following commands to get a list of available addons:

- eksctl utils describe-addon-versions --kubernetes-version 1.28 | grep AddonName

The following command will list available versions:

- eksctl utils describe-addon-versions --kubernetes-version 1.28 --name kube-proxy | grep AddonVersion

Determine if AWS or third party addon be presence of ProductUrl

- eksctl utils describe-addon-versions --kubernetes-version 1.28 --name name-of-addon | grep ProductUrl

The following command will install and overwrite self manages addons:

- eksctl create addon --cluster houston-eks --name kube-proxy --version v1.28.1-eksbuild.1 --service-account-role-arn arn:aws:iam::090378945367:role/eksClusterRole --force
- eksctl create addon --cluster houston-eks --name vpc-cni --version v1.14.1-eksbuild.1 --force

This needs done after the node group has been booted up.

- eksctl create addon --cluster houston-eks --name coredns --version v1.14.1-eksbuild.1 --force

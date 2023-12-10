# Houston Docker / Kubeneties Project
The following is an exploration project of using docker and k8s.  The outline below explains the structure and files.

- Batch Files -- these are here to document the Docker commands used.
-- create-network.bat & rm-network.bat -- create a "houston" network that all the docker containers will run in.  This will allow the docker containers to talk together, but be isolated from the rest of the world.  You'll need to expose contaier ports as needed.
-- create, start, stop, and rm containers -- these create the individual containers.  
- MessageBoard-DO NOT USE -- this is a boilerplate .NET WebAPI project in a docker container.
- htown-http -- this is a simple web server with a index.html copied into it to test creating custom docker containers
- htown-msg -- this is a message board web application in Visual Studio (not vscode).  This uses a database (mssql), web apis, and a simple html front end.  This will be the basis for what I'm looking to containerize and break apart.  There is a DB project that can be deployed to MS SQL to create your database.
- htown-mssql -- this is the mssql server used by htown-msg
- htown-ubuntu -- this is an ubuntu box on the houston network to troubleshoot with

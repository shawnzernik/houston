# Houston Docker / Kubeneties Project
The following is an exploration project of using docker and k8s.  The outline below explains the structure and files.

- Batch Files -- these are here to document the Docker commands used.
- - create-network.bat & rm-network.bat -- create a "houston" network that all the docker containers will run in.  This will allow the docker containers to talk together, but be isolated from the rest of the world.  You'll need to expose contaier ports as needed.
- - create, start, stop, and rm containers -- these create the individual containers.  
- MessageBoard-DO NOT USE -- this is a boilerplate .NET WebAPI project in a docker container.

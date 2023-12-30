#! /bin/sh

cd htown-msg

docker stop htown-msg
docker rm htown-msg

docker build -t szmsg .

docker create \
	--name htown-msg \
	--network houston \
	-p 8080:8080 \
	-e CONNECTION_STRING="Data Source=htown-mssql;Initial Catalog=MessageBoard;User ID=sa;Password=Welcome123;TrustServerCertificate=Yes" \
	szmsg

docker start htown-msg
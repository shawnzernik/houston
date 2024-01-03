#! /bin/sh

cd ../htown-msg

# docker stop htown-msg
# docker rm htown-msg

docker build -t szmsg .

# docker create \
# 	--name htown-msg \
# 	--network houston \
# 	-p 8080:8080 \
# 	-e CONNECTION_STRING="Data Source=htown-mssql;Initial Catalog=MessageBoard;User ID=sa;Password=Welcome123;TrustServerCertificate=Yes" \
# 	szmsg

# docker start htown-msg

export AWS_ACCESS_KEY_ID=AKIARKCYBINL4B3N45LM
export AWS_SECRET_ACCESS_KEY=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
export AWS_DEFAULT_REGION=us-east-2

aws ecr get-login-password --region us-east-2 | docker login --username AWS --password-stdin 090378945367.dkr.ecr.us-east-2.amazonaws.com

docker tag szmsg:latest 090378945367.dkr.ecr.us-east-2.amazonaws.com/szmsg:1.0.0
docker tag szmsg:latest 090378945367.dkr.ecr.us-east-2.amazonaws.com/szmsg:latest

docker push 090378945367.dkr.ecr.us-east-2.amazonaws.com/szmsg:1.0.0
docker push 090378945367.dkr.ecr.us-east-2.amazonaws.com/szmsg:latest

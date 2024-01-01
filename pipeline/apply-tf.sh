#! /bin/sh

export AWS_ACCESS_KEY_ID=AKIARKCYBINL4B3N45LM
export AWS_SECRET_ACCESS_KEY=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
export AWS_DEFAULT_REGION=us-east-2

# Terraform initialization
cd terraform

terraform init
terraform plan
terraform apply -auto-approve

cd ..

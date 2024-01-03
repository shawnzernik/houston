export AWS_ACCESS_KEY_ID=AKIARKCYBINL4B3N45LM
export AWS_SECRET_ACCESS_KEY=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
export AWS_DEFAULT_REGION=us-east-2

aws eks --region us-east-2 update-kubeconfig --name houston-eks
kubectl config current-context
kubectl apply -f kubernetes/houston-deployment.yaml
kubectl get svc szmsg-service
#kubectl delete -f szmsg-complete.yaml
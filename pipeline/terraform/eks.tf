################################################################################
# eksClusterRole
################################################################################
resource "aws_iam_role" "eks_cluster_role" {
  name        = "eksClusterRole"
  description = "Amazon EKS - Cluster role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Principal = {
          Service = ["eks.amazonaws.com", "ec2.amazonaws.com"]
        },
        Action = "sts:AssumeRole"
      }
    ]
  })

  tags = {
    application = "kube"
  }
}

resource "aws_iam_policy_attachment" "eks_cluster_policy_attachment" {
  name       = "eksClusterPolicyAttachment"
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSClusterPolicy"
  roles      = [aws_iam_role.eks_cluster_role.name]
}

resource "aws_iam_policy" "eks_ecr_access_policy" {
  name        = "eksECRAccessPolicy"
  description = "Policy to allow EKS cluster role to access ECR"

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Action = [
          "ecr:GetDownloadUrlForLayer",
          "ecr:BatchGetImage",
          "ecr:BatchCheckLayerAvailability"
        ],
        Resource = "*"
      }
    ]
  })
}

resource "aws_iam_policy_attachment" "eks_ecr_policy_attachment" {
  name       = "eksECRPolicyAttachment"
  policy_arn = aws_iam_policy.eks_ecr_access_policy.arn
  roles      = [aws_iam_role.eks_cluster_role.name]
}


################################################################################
# houston-security-group
################################################################################
resource "aws_security_group" "houston_security_group" {
  name        = "houston-security-group"
  description = "Houston Security Group"

  // Allow all inbound traffic
  ingress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]

  }

  // Allow all outbound traffic
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  // Add the "application" tag
  tags = {
    application = "kube"
  }
}

################################################################################
# EKS
################################################################################
resource "aws_eks_cluster" "houston-eks" {
  name     = "houston-eks"
  role_arn = aws_iam_role.eks_cluster_role.arn # Update this if necessary
  version  = "1.28"
  vpc_config {
    subnet_ids = [
      "subnet-0345524b5dc68093d",
      "subnet-029fb915098a9c089",
      "subnet-07fa1b93a76243f83"
    ]
    security_group_ids      = [aws_security_group.houston_security_group.id] # Use underscores here
    endpoint_public_access  = false
    endpoint_private_access = true
  }
  enabled_cluster_log_types = ["api", "audit", "authenticator", "controllerManager", "scheduler"]
  tags = {
    Name        = "houston-eks"
    application = "kube"
  }

  # Using create addon will give the following error: 
  #   - Addon kube_proxy specified is not supported in 1.28 kubernetes version
  # The kube-proxy was installed using a "self managed" version since this we not done through the web Management Console: 
  #   - Addon kube_proxy specified is not supported in 1.28 kubernetes version
  # Use the following command to create a managed kube-proxy
  #   - eksctl create addon --cluster houston-eks --name kube-proxy --version v1.28.1-eksbuild.1 --service-account-role-arn arn:aws:iam::090378945367:role/eksClusterRole --force
  provisioner "local-exec" {
    command = "eksctl create addon --cluster houston-eks --name kube-proxy --version v1.28.1-eksbuild.1 --service-account-role-arn arn:aws:iam::090378945367:role/eksClusterRole --force"
  }

  # Using create addon will give the following error: 
  #   - Addon vpc_cni specified is not supported in 1.28 kubernetes version
  # The kube-proxy was installed using a "self managed" version since this we not done through the web Management Console: 
  #   - Addon vpc_cni specified is not supported in 1.28 kubernetes version
  # Use the following command to create a managed kube-proxy
  #   - eksctl create addon --cluster houston-eks --name vpc-cni --version v1.14.1-eksbuild.1 --force
  provisioner "local-exec" {
    command = "eksctl create addon --cluster houston-eks --name vpc-cni --version v1.14.1-eksbuild.1 --force"
  }
}

################################################################################
# EKS Node Group
################################################################################
resource "aws_key_pair" "houston_node_group_keypair" {
  key_name   = "houston-node-group-keypair"
  public_key = file("${path.module}/houston-node-group-keypair.pub")

  tags = {
    application = "kube"
  }
}

resource "aws_eks_node_group" "houston_eks_node_group" {
  cluster_name    = aws_eks_cluster.houston-eks.name
  node_group_name = "houston-eks-node-group"
  node_role_arn   = aws_iam_role.eks_cluster_role.arn
  subnet_ids      = ["subnet-0345524b5dc68093d", "subnet-029fb915098a9c089", "subnet-07fa1b93a76243f83"]

  scaling_config {
    desired_size = 2
    max_size     = 4
    min_size     = 1
  }

  remote_access {
    ec2_ssh_key               = aws_key_pair.houston_node_group_keypair.key_name
    source_security_group_ids = [aws_security_group.houston_security_group.id]
  }

  instance_types = ["t2.medium", "t3.medium", "t3a.medium", "c5.large", "c5a.large", "c5ad.large", "c5d.large", "c6a.large", "c6i.large", "c6id.large", "c6in.large", "c7a.large", "c7i.large"] # 2CPU 4GB
  #capacity_type  = "SPOT"
  capacity_type = "ON_DEMAND"
  disk_size     = 20

  tags = {
    application = "kube"
  }

  depends_on = [
    aws_eks_cluster.houston-eks
  ]
}

resource "aws_eks_addon" "coredns" {
  cluster_name                = aws_eks_cluster.houston-eks.name
  addon_name                  = "coredns"
  addon_version               = "v1.10.1-eksbuild.2"
  resolve_conflicts_on_create = "OVERWRITE"

  depends_on = [ aws_eks_node_group.houston_eks_node_group ]

  configuration_values = jsonencode({
    replicaCount = 2
  })

  tags = {
    application = "kube"
  }
}

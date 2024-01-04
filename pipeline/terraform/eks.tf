################################################################################
# eksClusterRole
################################################################################
resource "aws_iam_role" "eksClusterRole" {
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
    Name        = "eksClusterRole"
    application = "kube"
  }
}

resource "aws_iam_policy_attachment" "eksAmazonEKSClusterPolicyAttachment" {
  name       = "eksAmazonEKSClusterPolicyAttachment"
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSClusterPolicy"
  roles      = [aws_iam_role.eksClusterRole.name]
}

resource "aws_iam_policy_attachment" "eksAmazonEKS_CNI_PolicyAttachment" {
  name       = "eksAmazonEKS_CNI_PolicyAttachment"
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKS_CNI_Policy"
  roles      = [aws_iam_role.eksClusterRole.name]
}

resource "aws_iam_policy_attachment" "eksAmazonEKSServicePolicyAttachment" {
  name       = "eksAmazonEKSServicePolicyAttachment"
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSServicePolicy"
  roles      = [aws_iam_role.eksClusterRole.name]
}

resource "aws_iam_policy_attachment" "eksAmazonEKSVPCResourceControllerAttachment" {
  name       = "eksAmazonEKSVPCResourceControllerAttachment"
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSVPCResourceController"
  roles      = [aws_iam_role.eksClusterRole.name]
}

resource "aws_iam_policy_attachment" "eksAmazonEKSWorkerNodePolicyAttachment" {
  name       = "eksAmazonEKSWorkerNodePolicyAttachment"
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKSWorkerNodePolicy"
  roles      = [aws_iam_role.eksClusterRole.name]
}

resource "aws_iam_policy" "eksEcrAccessPolicy" {
  name        = "eksEcrAccessPolicy"
  description = "Policy to allow EKS cluster role to access ECR"

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Action = [
          "ecr:GetAuthorizationToken",
          "ecr:BatchCheckLayerAvailability",
          "ecr:GetDownloadUrlForLayer",
          "ecr:BatchGetImage",
          "ecr:DescribeRepositories",
          "ecr:ListImages",
          "ecr:DescribeImages",
          "ecr:GetRepositoryPolicy",
          "ecr:ListTagsForResource",
          "ecr:DescribeImageScanFindings"
        ],
        Resource = "*"
      }
    ]
  })

  tags = {
    Name        = "eksEcrAccessPolicy"
    application = "kube"
  }
}

resource "aws_iam_policy_attachment" "eksEcrPolicyAttachment" {
  name       = "eksEcrPolicyAttachment"
  policy_arn = aws_iam_policy.eksEcrAccessPolicy.arn
  roles      = [aws_iam_role.eksClusterRole.name]
}


################################################################################
# houston-security-group
################################################################################
# resource "aws_security_group" "houston-security-group" {
#   name        = "houston-security-group"
#   description = "Houston Security Group"

#   // Allow all inbound traffic
#   ingress {
#     from_port   = 0
#     to_port     = 0
#     protocol    = "-1"
#     cidr_blocks = ["0.0.0.0/0"]

#   }

#   // Allow all outbound traffic
#   egress {
#     from_port   = 0
#     to_port     = 0
#     protocol    = "-1"
#     cidr_blocks = ["0.0.0.0/0"]
#   }

#   // Add the "application" tag
#   tags = {
#     application = "kube"
#   }
# }

################################################################################
# EKS
################################################################################
resource "aws_eks_cluster" "houston-eks" {
  name     = "houston-eks"
  role_arn = aws_iam_role.eksClusterRole.arn # Update this if necessary
  version  = "1.28"

  vpc_config {
    subnet_ids = [
      "subnet-0345524b5dc68093d",
      "subnet-029fb915098a9c089",
      "subnet-07fa1b93a76243f83"
    ]
    # security_group_ids      = [aws_security_group.houston-security-group.id] # Use underscores here
    endpoint_public_access  = false
    endpoint_private_access = true
  }

  enabled_cluster_log_types = ["api", "audit", "authenticator", "controllerManager", "scheduler"]

  tags = {
    Name        = "houston-eks"
    application = "kube"
  }

  kubernetes_network_config {
    service_ipv4_cidr = "10.100.0.0/16"
    ip_family         = "ipv4"
  }

  # # Using create addon will give the following error: 
  # #   - Addon kube_proxy specified is not supported in 1.28 kubernetes version
  # # The kube-proxy was installed using a "self managed" version since this we not done through the web Management Console: 
  # #   - Addon kube_proxy specified is not supported in 1.28 kubernetes version
  # # Use the following command to create a managed kube-proxy
  # #   - eksctl create addon --cluster houston-eks --name kube-proxy --version v1.28.1-eksbuild.1 --service-account-role-arn arn:aws:iam::090378945367:role/eksClusterRole --force
  # provisioner "local-exec" {
  #   command = "eksctl create addon --cluster houston-eks --name kube-proxy --version v1.28.1-eksbuild.1 --service-account-role-arn arn:aws:iam::090378945367:role/eksClusterRole --force"
  # }

  # # Using create addon will give the following error: 
  # #   - Addon vpc_cni specified is not supported in 1.28 kubernetes version
  # # The kube-proxy was installed using a "self managed" version since this we not done through the web Management Console: 
  # #   - Addon vpc_cni specified is not supported in 1.28 kubernetes version
  # # Use the following command to create a managed kube-proxy
  # #   - eksctl create addon --cluster houston-eks --name vpc-cni --version v1.14.1-eksbuild.1 --force
  # provisioner "local-exec" {
  #   command = "eksctl create addon --cluster houston-eks --name vpc-cni --version v1.14.1-eksbuild.1 --force"
  # }
}

################################################################################
# EKS Node Group
################################################################################
resource "aws_key_pair" "houston-node-group-keypair" {
  key_name   = "houston-node-group-keypair"
  public_key = file("${path.module}/houston-node-group-keypair.pub")

  tags = {
    Name        = "houston-node-group-keypair"
    application = "kube"
  }
}

resource "aws_eks_node_group" "houston-eks-node-group" {
  cluster_name    = aws_eks_cluster.houston-eks.name
  node_group_name = "houston-eks-node-group"
  node_role_arn   = aws_iam_role.eksClusterRole.arn
  subnet_ids = [
    "subnet-0345524b5dc68093d",
    "subnet-029fb915098a9c089",
    "subnet-07fa1b93a76243f83"
  ]

  scaling_config {
    desired_size = 2
    max_size     = 4
    min_size     = 1
  }

  remote_access {
    ec2_ssh_key = aws_key_pair.houston-node-group-keypair.key_name
    # source_security_group_ids = [aws_security_group.houston-security-group.id]
  }

  instance_types = [
    # # 2CPU GB @ $0.02/hr
    # "t3a.small",
    # "t3.small"

    # 2CPU 4GB @ 0.04/hr
    "t2.medium",
    "t3.medium",
    "t3a.medium"
  ]

  capacity_type = "SPOT"
  #capacity_type = "ON_DEMAND"

  disk_size = 20

  tags = {
    Name        = "houston-eks-node-group"
    application = "kube"
  }

  depends_on = [
    aws_eks_cluster.houston-eks
  ]
}

# resource "aws_eks_addon" "coredns" {
#   cluster_name                = aws_eks_cluster.houston-eks.name
#   addon_name                  = "coredns"
#   addon_version               = "v1.10.1-eksbuild.2"
#   resolve_conflicts_on_create = "OVERWRITE"

#   depends_on = [aws_eks_node_group.houston-eks-node-group]

#   configuration_values = jsonencode({
#     replicaCount = 2
#   })

#   tags = {
#     application = "kube"
#   }
# }

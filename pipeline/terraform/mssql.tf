# Define a security group for RDS
resource "aws_security_group" "rds_security_group" {
  name        = "mssql-security-group"
  description = "Allow incoming traffic to RDS"

  vpc_id = "vpc-0d0bd9436813510fa"

  ingress {
    from_port   = 1433
    to_port     = 1433
    protocol    = "tcp"
    cidr_blocks = ["172.31.0.0/16"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    application = "mssql"
  }
}

# Define RDS instance with SQL Server Express 2022 and 20GB storage
resource "aws_db_instance" "messageboard_database" {
  identifier            = "messageboard-database"
  allocated_storage    = 20
  storage_type          = "gp2"
  engine                = "sqlserver-ex"
  engine_version        = "16.00.4095.4.v1"
  instance_class        = "db.t3.small"
  username              = "admin"
  password              = "Welcome123"

  vpc_security_group_ids = [aws_security_group.rds_security_group.id]
  db_subnet_group_name   = aws_db_subnet_group.messageboard_db_subnet_group.name

  backup_retention_period = 7
  backup_window           = "05:00-09:00"

  skip_final_snapshot = true

  tags = {
    application = "mssql"
    Name        = "messageboard-database"
  }
}

# Define DB subnet group with additional subnets
resource "aws_db_subnet_group" "messageboard_db_subnet_group" {
  name       = "messageboard-db-subnet-group"
  subnet_ids = [
    "subnet-029fb915098a9c089", # Existing subnet
    "subnet-0345524b5dc68093d", # Additional subnet
    "subnet-07fa1b93a76243f83", # Additional subnet
  ]

  tags = {
    application = "mssql"
    Name        = "messageboard-db-subnet-group"
  }
}

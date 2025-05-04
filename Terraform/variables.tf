variable "do_token" {
  description = "DigitalOcean API token"
  type        = string
  sensitive   = true
}

variable "project_name" {
  description = "Project name"
  type        = string
  default     = "syki"
}

variable "environment" {
  description = "Environment"
  type        = string
  default     = "development"
}

variable "region" {
  description = "DigitalOcean region"
  type        = string
  default     = "nyc1"
}

variable "kubernetes_version" {
  description = "Kubernetes version"
  type        = string
  default     = "1.28"
}

variable "node_pool_size" {
  description = "Node pool instance size"
  type        = string
  default     = "s-1vcpu-2gb"
}

variable "node_count" {
  description = "Number of nodes in the cluster"
  type        = number
  default     = 1
}

variable "db_size" {
  description = "Database instance size"
  type        = string
  default     = "db-s-1vcpu-1gb"
}

variable "postgres_user" {
  description = "Database username"
  type        = string
}

variable "postgres_password" {
  description = "Password for PostgreSQL in container"
  type        = string
  sensitive   = true
}

variable "grafana_password" {
  description = "Grafana admin password"
  type        = string
  sensitive   = true
  default     = "admin"
}

variable "enable_cdn" {
  description = "Enable CDN for frontend"
  type        = bool
  default     = false
}

variable "storage_size" {
  description = "Storage volume size in GB"
  type        = number
  default     = 1
}

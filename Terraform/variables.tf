variable "resource_group_name" {
  description = "Resource group name for SYKI project"
  default     = "syki-rg"
}

variable "location" {
  description = "Azure region to deploy resources"
  default     = "brazilsouth"
}

variable "cluster_name" {
  description = "Name of the AKS cluster"
  default     = "syki-aks"
}

variable "dns_prefix" {
  description = "DNS prefix for the AKS cluster"
  default     = "syki"
}

variable "kubernetes_version" {
  description = "Kubernetes version"
  default     = "1.32.4"
}

variable "acr_name" {
  description = "Name for the Azure Container Registry"
  default     = "sykiacr"
}

variable "node_count" {
  description = "Number of nodes in the default AKS node pool"
  default     = 1
}

variable "node_vm_size" {
  description = "VM size for the AKS nodes"
  default     = "Standard_DS2_v2"
}

# Version constraint for the provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "syki" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_log_analytics_workspace" "syki" {
  name                = "${var.cluster_name}-logs"
  location            = azurerm_resource_group.syki.location
  resource_group_name = azurerm_resource_group.syki.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_kubernetes_cluster" "syki" {
  name                = var.cluster_name
  location            = azurerm_resource_group.syki.location
  resource_group_name = azurerm_resource_group.syki.name
  dns_prefix          = var.dns_prefix
  kubernetes_version  = var.kubernetes_version

  default_node_pool {
    name           = "default"
    node_count     = var.node_count
    vm_size        = var.node_vm_size
    os_disk_size_gb = 30
  }

  identity {
    type = "SystemAssigned"
  }
  
  oms_agent {
    log_analytics_workspace_id = azurerm_log_analytics_workspace.syki.id
  }

  tags = {
    Environment = "Education"
    Project     = "SYKI"
  }
}

resource "azurerm_kubernetes_cluster_node_pool" "spot" {
  name                  = "spot"
  kubernetes_cluster_id = azurerm_kubernetes_cluster.syki.id
  vm_size               = "Standard_B2s"
  node_count            = 1
  
  node_labels = {
    "kubernetes.azure.com/scalesetpriority" = "spot"
  }
  
  node_taints = ["kubernetes.azure.com/scalesetpriority=spot:NoSchedule"]
  
  priority        = "Spot"
  eviction_policy = "Delete"
  spot_max_price  = -1  # -1 means use on-demand price as max
  
  tags = {
    Environment = "Education"
    Project     = "SYKI"
  }
}

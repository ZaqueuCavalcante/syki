# Azure provider config - - - - - - - - - - - - - - - - - - - - - //
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.2.0"
    }
  }
}
provider "azurerm" {
  features {}
  subscription_id = "989a2a28-635f-4718-a706-ffbc2451c402"
}

# Resource group - - - - - - - - - - - - - - - - - - - - - - - - //
resource "azurerm_resource_group" "syki_rg" {
  name     = "syki-rg"
  location = "Brazil South"
}

# Network - - - - - - - - - - - - - - - - - - - - - - - - - - - - //
resource "azurerm_network_security_group" "syki_nsg" {
  name                = "syki-nsg"
  resource_group_name = azurerm_resource_group.syki_rg.name
  location            = azurerm_resource_group.syki_rg.location
}
resource "azurerm_virtual_network" "syki_vn" {
  name                = "syki-vn"
  resource_group_name = azurerm_resource_group.syki_rg.name
  location            = azurerm_resource_group.syki_rg.location
  address_space       = ["10.0.0.0/16"]
  dns_servers         = ["10.0.0.4", "10.0.0.5"]
  subnet {
    name             = "subnet-1"
    address_prefixes = ["10.0.1.0/24"]
  }
  subnet {
    name             = "subnet-2"
    address_prefixes = ["10.0.2.0/24"]
    security_group   = azurerm_network_security_group.syki_nsg.id
  }
}

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
resource "azurerm_network_security_group" "syki_nsg_hub" {
  name                = "syki-nsg-hub"
  resource_group_name = azurerm_resource_group.syki_rg.name
  location            = azurerm_resource_group.syki_rg.location
}
resource "azurerm_virtual_network" "syki_vn_hub" {
  name                = "syki-vn-hub"
  resource_group_name = azurerm_resource_group.syki_rg.name
  location            = azurerm_resource_group.syki_rg.location
  address_space       = ["10.10.0.0/16"]

  subnet {
    name             = "subnet-1"
    address_prefixes = ["10.10.1.0/24"]
    security_group   = azurerm_network_security_group.syki_nsg_hub.id
  }
  subnet {
    name             = "subnet-2"
    address_prefixes = ["10.10.2.0/24"]
    security_group   = azurerm_network_security_group.syki_nsg_hub.id
  }
}

resource "azurerm_network_security_group" "syki_nsg_spoke" {
  name                = "syki-nsg-spoke"
  resource_group_name = azurerm_resource_group.syki_rg.name
  location            = azurerm_resource_group.syki_rg.location
}
resource "azurerm_virtual_network" "syki_vn_spoke" {
  name                = "syki-vn-spoke"
  resource_group_name = azurerm_resource_group.syki_rg.name
  location            = azurerm_resource_group.syki_rg.location
  address_space       = ["10.11.0.0/16"]

  subnet {
    name             = "subnet-1"
    address_prefixes = ["10.11.1.0/24"]
    security_group   = azurerm_network_security_group.syki_nsg_spoke.id
  }
}

# Services - - - - - - - - - - - - - - - - - - - - - - - - - - - - //
resource "azurerm_service_plan" "syki_sp" {
  name                = "syki-sp"
  resource_group_name = azurerm_resource_group.syki_rg.name
  location            = azurerm_resource_group.syki_rg.location
  os_type             = "Linux"
  sku_name            = "P0v3"
}
resource "azurerm_linux_web_app" "syki_lwa" {
  name                = "syki-lwa"
  resource_group_name = azurerm_resource_group.syki_rg.name
  location            = azurerm_service_plan.syki_sp.location
  service_plan_id     = azurerm_service_plan.syki_sp.id

  site_config {}
}

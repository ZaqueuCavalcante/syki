# Azure provider config - - - - - - - - - - - - - - - - - - - - - //

terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "4.2.0"
    }
  }
}

provider "azurerm" {
  features {}
  subscription_id = "989a2a28-635f-4718-a706-ffbc2451c402"
}

# Resource group - - - - - - - - - - - - - - - - - - - - - - - - - - - - //

resource "azurerm_resource_group" "syki_resource_group" {
  name     = "syki_resource_group"
  location = "Brazil South"
}

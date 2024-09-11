terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.1.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.region
}

resource "azurerm_app_service_plan" "asp" {
    name                = var.app_service_plan_name
    location            = var.region
    resource_group_name = azurerm_resource_group.rg.name

    sku {
      tier = "Standard"
      size = "S1"
    }

    kind     = "Linux"
    reserved = true
}

resource "azurerm_linux_web_app" "app" {
    name                = var.web_app_name
    resource_group_name = azurerm_resource_group.rg.name
    location            = azurerm_resource_group.rg.location
    service_plan_id     = azurerm_app_service_plan.asp.id

    site_config {
      app_command_line = ""
    }

    app_settings = {
      "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    }
}

# https://blog.stackademic.com/deploying-a-web-app-to-azure-with-terraform-integrating-infrastructure-as-code-into-ci-cd-862013ba3885

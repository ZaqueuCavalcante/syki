variable "resource_group_name" {
  type = string
  default = "SykiResourceGroup"
}

variable "region" {
  type = string
  default = "brazilsouth"
}

variable "app_service_plan_name" {
  type = string
  default = "SykiAppServicePlan"
}

variable "web_app_name" {
  type = string
  default = "SykiApi"
}

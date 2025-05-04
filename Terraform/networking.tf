resource "digitalocean_vpc" "vpc" {
  name     = "${var.project_name}-dev-vpc"
  region   = var.region
  ip_range = "10.10.0.0/16"
}
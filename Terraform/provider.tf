terraform {
  required_providers {
    digitalocean = {
      source  = "digitalocean/digitalocean"
      version = "~> 2.54.0"
    }
  }
}

provider "digitalocean" {
  token = var.do_token
}
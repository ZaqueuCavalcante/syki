resource "digitalocean_droplet" "app" {
  image    = "ubuntu-22-04-x64"
  name     = "syki-app"
  region   = var.region
  size     = "s-2vcpu-2gb"
  ssh_keys = [data.digitalocean_ssh_key.main.id]

  user_data = templatefile("${path.module}/Scripts/cloud-init.yaml", {
    postgres_password      = var.postgres_password
    grafana_admin_password = var.grafana_admin_password
  })
}

data "digitalocean_ssh_key" "main" {
  name = var.ssh_key_name
}
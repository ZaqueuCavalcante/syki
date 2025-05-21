resource "digitalocean_droplet" "app" {
  image    = "ubuntu-22-04-x64"
  name     = "syki-app"
  region   = var.region
  size     = "s-1vcpu-1gb"
  ssh_keys = [data.digitalocean_ssh_key.main.id]

  user_data = templatefile("${path.module}/Scripts/cloud-init.yaml", {
    postgres_password      = var.postgres_password
    grafana_admin_password = var.grafana_admin_password
  })
}

#resource "digitalocean_firewall" "app" {
#  name = "syki-firewall"
#
#  droplet_ids = [digitalocean_droplet.app.id]
#
#  inbound_rule {
#    protocol         = "tcp"
#    port_range       = "22"
#    source_addresses = ["0.0.0.0/0", "::/0"]
#  }
#
#  inbound_rule {
#    protocol         = "tcp"
#    port_range       = "5001-5003"
#    source_addresses = ["0.0.0.0/0", "::/0"]
#  }
#
#  inbound_rule {
#    protocol         = "tcp"
#    port_range       = "5432"
#    source_addresses = ["0.0.0.0/0", "::/0"]
#  }
#
#  inbound_rule {
#    protocol         = "tcp"
#    port_range       = "3000"
#    source_addresses = ["0.0.0.0/0", "::/0"]
#  }
#}

data "digitalocean_ssh_key" "main" {
  name = var.ssh_key_name
}
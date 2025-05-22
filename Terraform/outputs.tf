output "droplet_ip" {
  value = digitalocean_droplet.app.ipv4_address
}

output "app_urls" {
  value = {
    frontend  = "http://${digitalocean_droplet.app.ipv4_address}:5002"
    backend   = "http://${digitalocean_droplet.app.ipv4_address}:5001"
    daemon    = "http://${digitalocean_droplet.app.ipv4_address}:5003"
    grafana   = "http://${digitalocean_droplet.app.ipv4_address}:3000"
    database  = "${digitalocean_droplet.app.ipv4_address}:5432"
  }
}
resource "digitalocean_droplet" "app" {
  image  = "ubuntu-22-04-x64"
  name   = "syki-app"
  region = var.region
  size   = "s-1vcpu-2gb-amd"
  ssh_keys = [data.digitalocean_ssh_key.main.id]

  user_data = <<-EOF
    #!/bin/bash
    apt-get update
    apt-get install -y python3
  EOF

  provisioner "remote-exec" {
    inline = ["echo 'SSH connection established'"]

    connection {
      type        = "ssh"
      user        = "root"
      host        = self.ipv4_address
      private_key = file(var.ssh_private_key_path)
    }
  }

  provisioner "local-exec" {
    command = "cd ${path.module}/ansible && ANSIBLE_HOST_KEY_CHECKING=False ansible-playbook -i inventory/hosts.yml playbook.yml -e 'ansible_host=${self.ipv4_address}' -e 'postgres_password=${var.postgres_password}' -e 'grafana_admin_password=${var.grafana_admin_password}'"
  }
}

data "digitalocean_ssh_key" "main" {
  name = var.ssh_key_name
}
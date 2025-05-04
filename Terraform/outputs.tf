output "kubernetes_cluster_name" {
  value = digitalocean_kubernetes_cluster.primary.name
}

output "kubernetes_endpoint" {
  value     = digitalocean_kubernetes_cluster.primary.endpoint
  sensitive = true
}

output "kubeconfig" {
  value     = digitalocean_kubernetes_cluster.primary.kube_config[0].raw_config
  sensitive = true
}

output "postgres_service" {
  value = kubernetes_service.postgres.metadata[0].name
}

output "postgres_connection_string" {
  value     = "postgresql://${var.project_name}_user:${var.postgres_password}@postgres.${kubernetes_namespace.app.metadata[0].name}.svc.cluster.local:5432/${var.project_name}_db"
  sensitive = true
}

output "k8s_node_ip" {
  value = "Execute: kubectl get nodes -o wide para obter o IP do nó para acesso via NodePort"
}

output "grafana_nodeport" {
  value = "Exponha o Grafana com: kubectl port-forward svc/prometheus-grafana 3000:80 -n monitoring"
}

output "db_connection_string" {
  value     = "postgresql://${digitalocean_database_user.app_user.name}:${digitalocean_database_user.app_user.password}@${digitalocean_database_cluster.postgres.host}:${digitalocean_database_cluster.postgres.port}/${var.project_name}_db?sslmode=require"
  sensitive = true
}
output "resource_group_name" {
  value = azurerm_resource_group.syki.name
}

output "kubernetes_cluster_name" {
  value = azurerm_kubernetes_cluster.syki.name
}

output "postgresql_password" {
  value     = random_password.pg_password.result
  sensitive = true
}

output "grafana_admin_password" {
  value     = random_password.grafana_password.result
  sensitive = true
}

output "front_service_ip" {
  value = kubernetes_service.front.status[0].load_balancer[0].ingress[0].ip
}

output "grafana_access_instructions" {
  value = "Para acessar o Grafana, execute: kubectl get svc -n monitoring prometheus-grafana -o jsonpath='{.status.loadBalancer.ingress[0].ip}'"
  description = "Instruções para obter o endereço IP do Grafana"
  depends_on = [helm_release.prometheus]
}

output "kubernetes_command" {
  value = "az aks get-credentials --resource-group ${azurerm_resource_group.syki.name} --name ${azurerm_kubernetes_cluster.syki.name}"
}

output "container_registry_login_server" {
  value = azurerm_container_registry.acr.login_server
}

output "container_registry_admin_username" {
  value = azurerm_container_registry.acr.admin_username
}

output "container_registry_admin_password" {
  value     = azurerm_container_registry.acr.admin_password
  sensitive = true
}

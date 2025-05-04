resource "digitalocean_kubernetes_cluster" "primary" {
  name    = "${var.project_name}-${var.environment}-cluster"
  region  = var.region
  version = var.kubernetes_version

  node_pool {
    name       = "default-pool"
    size       = "s-1vcpu-2gb"
    node_count = 1
  }
}

resource "kubernetes_namespace" "app" {
  depends_on = [digitalocean_kubernetes_cluster.primary]
  
  metadata {
    name = var.project_name
  }
}

resource "kubernetes_namespace" "monitoring" {
  depends_on = [digitalocean_kubernetes_cluster.primary]
  
  metadata {
    name = "monitoring"
  }
}

resource "kubernetes_secret" "db_connection" {
  metadata {
    name      = "db-connection"
    namespace = kubernetes_namespace.app.metadata[0].name
  }

  data = {
    DB_CONNECTION_STRING = base64encode("postgresql://${digitalocean_database_user.app_user.name}:${digitalocean_database_user.app_user.password}@${digitalocean_database_cluster.postgres.host}:${digitalocean_database_cluster.postgres.port}/${var.project_name}_db?sslmode=require")
  }
}

resource "helm_release" "prometheus_stack" {
  depends_on = [kubernetes_namespace.monitoring]
  
  name       = "prometheus"
  repository = "https://prometheus-community.github.io/helm-charts"
  chart      = "kube-prometheus-stack"
  namespace  = "monitoring"
  
  set {
    name  = "grafana.adminPassword"
    value = var.grafana_password
  }
  
  set {
    name  = "prometheus.prometheusSpec.resources.requests.cpu"
    value = "100m"
  }
  set {
    name  = "prometheus.prometheusSpec.resources.requests.memory"
    value = "256Mi"
  }
  set {
    name  = "prometheus.prometheusSpec.resources.limits.cpu"
    value = "200m"
  }
  set {
    name  = "prometheus.prometheusSpec.resources.limits.memory"
    value = "512Mi"
  }

  set {
    name  = "prometheus.prometheusSpec.retention"
    value = "1d"
  }
  
  set {
    name  = "prometheus.prometheusSpec.storageSpec.volumeClaimTemplate.spec.resources.requests.storage"
    value = "1Gi"
  }
}

resource "helm_release" "loki_stack" {
  depends_on = [kubernetes_namespace.monitoring]
  
  name       = "loki"
  repository = "https://grafana.github.io/helm-charts"
  chart      = "loki-stack"
  namespace  = "monitoring"
  
  set {
    name  = "grafana.enabled"
    value = "false"
  }
  
  set {
    name  = "loki.persistence.enabled"
    value = "true"
  }
  
  set {
    name  = "loki.persistence.size"
    value = "1Gi"
  }
  
  set {
    name  = "loki.resources.requests.cpu"
    value = "50m"
  }
  set {
    name  = "loki.resources.requests.memory"
    value = "128Mi"
  }
}

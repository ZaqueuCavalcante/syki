provider "helm" {
  kubernetes {
    host                   = azurerm_kubernetes_cluster.syki.kube_config.0.host
    client_certificate     = base64decode(azurerm_kubernetes_cluster.syki.kube_config.0.client_certificate)
    client_key             = base64decode(azurerm_kubernetes_cluster.syki.kube_config.0.client_key)
    cluster_ca_certificate = base64decode(azurerm_kubernetes_cluster.syki.kube_config.0.cluster_ca_certificate)
  }
}

resource "kubernetes_namespace" "monitoring" {
  metadata {
    name = "monitoring"
  }
}

resource "helm_release" "prometheus" {
  name       = "prometheus"
  repository = "https://prometheus-community.github.io/helm-charts"
  chart      = "kube-prometheus-stack"
  namespace  = kubernetes_namespace.monitoring.metadata[0].name
  version    = "51.2.0"

  values = [<<-EOT
    prometheus:
      service:
        type: ClusterIP
      server:
        resources:
          limits:
            cpu: 500m
            memory: 512Mi
          requests:
            cpu: 200m
            memory: 256Mi
        persistentVolume:
          enabled: true
          size: 8Gi
    
    grafana:
      service:
        type: LoadBalancer
      adminPassword: "${random_password.grafana_password.result}"
      resources:
        limits:
          cpu: 200m
          memory: 256Mi
        requests:
          cpu: 100m
          memory: 128Mi
      persistence:
        enabled: true
        size: 2Gi
      dashboardProviders:
        default:
          disableDeletion: false
          options:
            path: /var/lib/grafana/dashboards/default
    EOT
  ]
}

resource "helm_release" "loki_stack" {
  name       = "loki"
  repository = "https://grafana.github.io/helm-charts"
  chart      = "loki-stack"
  namespace  = kubernetes_namespace.monitoring.metadata[0].name
  version    = "2.9.11"

  values = [<<-EOT
    loki:
      persistence:
        enabled: true
        size: 5Gi
      resources:
        limits:
          cpu: 200m
          memory: 256Mi
        requests:
          cpu: 100m
          memory: 128Mi
  EOT
  ]

  depends_on = [helm_release.prometheus]
}

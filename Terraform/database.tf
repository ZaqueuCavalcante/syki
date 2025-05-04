resource "kubernetes_deployment" "postgres" {
  depends_on = [digitalocean_kubernetes_cluster.primary]

  metadata {
    name      = "postgres"
    namespace = kubernetes_namespace.app.metadata[0].name
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        app = "postgres"
      }
    }

    template {
      metadata {
        labels = {
          app = "postgres"
        }
      }

      spec {
        container {
          image = "postgres:15"
          name  = "postgres"

          resources {
            limits = {
              cpu    = "500m"
              memory = "512Mi"
            }
            requests = {
              cpu    = "200m"
              memory = "256Mi"
            }
          }

          env {
            name  = "POSTGRES_DB"
            value = "${var.project_name}_db"
          }
          
          env {
            name  = "POSTGRES_USER"
            value = "${var.project_name}_user"
          }

          env {
            name  = "POSTGRES_PASSWORD"
            value = var.postgres_password
          }

          port {
            container_port = 5432
          }

          volume_mount {
            name       = "postgres-storage"
            mount_path = "/var/lib/postgresql/data"
          }
        }

        volume {
          name = "postgres-storage"
          persistent_volume_claim {
            claim_name = kubernetes_persistent_volume_claim.postgres.metadata[0].name
          }
        }
      }
    }
  }
}

resource "kubernetes_persistent_volume_claim" "postgres" {
  depends_on = [digitalocean_kubernetes_cluster.primary]
  
  metadata {
    name      = "postgres-data"
    namespace = kubernetes_namespace.app.metadata[0].name
  }
  spec {
    access_modes = ["ReadWriteOnce"]
    resources {
      requests = {
        storage = "1Gi"
      }
    }
    storage_class_name = "do-block-storage"
  }
}

resource "kubernetes_service" "postgres" {
  depends_on = [digitalocean_kubernetes_cluster.primary]
  
  metadata {
    name      = "postgres"
    namespace = kubernetes_namespace.app.metadata[0].name
  }
  spec {
    selector = {
      app = "postgres"
    }
    port {
      port        = 5432
      target_port = 5432
    }
  }
}
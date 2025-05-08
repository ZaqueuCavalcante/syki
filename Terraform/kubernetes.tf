provider "kubernetes" {
  host                   = azurerm_kubernetes_cluster.syki.kube_config.0.host
  client_certificate     = base64decode(azurerm_kubernetes_cluster.syki.kube_config.0.client_certificate)
  client_key             = base64decode(azurerm_kubernetes_cluster.syki.kube_config.0.client_key)
  cluster_ca_certificate = base64decode(azurerm_kubernetes_cluster.syki.kube_config.0.cluster_ca_certificate)
}

resource "kubernetes_namespace" "syki" {
  metadata {
    name = "syki"
  }
}

# Postgres Secret
resource "kubernetes_secret" "postgres_secret" {
  metadata {
    name      = "postgres-secret"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }

  data = {
    POSTGRES_PASSWORD = random_password.pg_password.result
    POSTGRES_USER     = "postgres"
    POSTGRES_DB       = "syki-db"
  }
}

resource "kubernetes_persistent_volume_claim" "postgres_pvc" {
  metadata {
    name      = "postgres-data"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }
  spec {
    access_modes = ["ReadWriteOnce"]
    resources {
      requests = {
        storage = "1Gi"
      }
    }
  }
}

# Postgres Deployment
resource "kubernetes_deployment" "postgres" {
  metadata {
    name      = "postgres"
    namespace = kubernetes_namespace.syki.metadata[0].name
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
          image = "postgres:17.4-alpine3.21"
          name  = "postgres"
          
          resources {
            limits = {
              cpu    = "0.5"
              memory = "512Mi"
            }
            requests = {
              cpu    = "0.2"
              memory = "256Mi"
            }
          }
          
          port {
            container_port = 5432
          }
          
          env {
            name = "POSTGRES_USER"
            value_from {
              secret_key_ref {
                name = kubernetes_secret.postgres_secret.metadata[0].name
                key  = "POSTGRES_USER"
              }
            }
          }
          
          env {
            name = "POSTGRES_PASSWORD"
            value_from {
              secret_key_ref {
                name = kubernetes_secret.postgres_secret.metadata[0].name
                key  = "POSTGRES_PASSWORD"
              }
            }
          }
          
          env {
            name = "POSTGRES_DB"
            value_from {
              secret_key_ref {
                name = kubernetes_secret.postgres_secret.metadata[0].name
                key  = "POSTGRES_DB"
              }
            }
          }
          
          env {
            name  = "PGDATA"
            value = "/var/lib/postgresql/data/pgdata"
          }
          
          args = [
            "postgres",
            "-c", "max_connections=1500",
            "-c", "max_prepared_transactions=64",
            "-c", "log_statement=all"
          ]
          
          volume_mount {
            name       = "postgres-data"
            mount_path = "/var/lib/postgresql/data"
          }
          
          liveness_probe {
            exec {
              command = ["pg_isready", "-U", "postgres", "-d", "syki-db"]
            }
            initial_delay_seconds = 30
            period_seconds        = 10
            timeout_seconds       = 5
            failure_threshold     = 6
          }
          
          readiness_probe {
            exec {
              command = ["pg_isready", "-U", "postgres", "-d", "syki-db"]
            }
            initial_delay_seconds = 5
            period_seconds        = 5
            timeout_seconds       = 3
            failure_threshold     = 3
          }
        }
        
        volume {
          name = "postgres-data"
          persistent_volume_claim {
            claim_name = kubernetes_persistent_volume_claim.postgres_pvc.metadata[0].name
          }
        }
      }
    }
  }
}

# Postgres Service
resource "kubernetes_service" "postgres" {
  metadata {
    name      = "postgres"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }
  spec {
    selector = {
      app = kubernetes_deployment.postgres.spec[0].template[0].metadata[0].labels.app
    }
    port {
      port        = 5432
      target_port = 5432
    }
  }
}

# Back Deployment
resource "kubernetes_deployment" "back" {
  metadata {
    name      = "back"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        app = "back"
      }
    }

    template {
      metadata {
        labels = {
          app = "back"
        }
      }

      spec {
        container {
          image = "${azurerm_container_registry.acr.login_server}/syki-back:latest"
          name  = "back"
          
          resources {
            limits = {
              cpu    = "0.5"
              memory = "512Mi"
            }
            requests = {
              cpu    = "0.2"
              memory = "256Mi"
            }
          }
          
          port {
            container_port = 8080
          }
          
          env {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = "Development"
          }
          
          env {
            name  = "Database__ConnectionString"
            value = "UserID=postgres;Password=${random_password.pg_password.result};Host=postgres;Port=5432;Database=syki-db;Pooling=true;"
          }
          
          liveness_probe {
            http_get {
              path = "/health"
              port = 8080
            }
            initial_delay_seconds = 30
            period_seconds        = 10
          }
          
          readiness_probe {
            http_get {
              path = "/health"
              port = 8080
            }
            initial_delay_seconds = 15
            period_seconds        = 5
          }
        }
      }
    }
  }
}

# Back Service
resource "kubernetes_service" "back" {
  metadata {
    name      = "back"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }
  spec {
    selector = {
      app = kubernetes_deployment.back.spec[0].template[0].metadata[0].labels.app
    }
    port {
      port        = 80
      target_port = 8080
    }
  }
}

# Front Deployment
resource "kubernetes_deployment" "front" {
  metadata {
    name      = "front"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        app = "front"
      }
    }

    template {
      metadata {
        labels = {
          app = "front"
        }
      }

      spec {
        container {
          image = "${azurerm_container_registry.acr.login_server}/syki-front:latest"
          name  = "front"
          
          resources {
            limits = {
              cpu    = "0.3"
              memory = "256Mi"
            }
            requests = {
              cpu    = "0.1"
              memory = "128Mi"
            }
          }
          
          port {
            container_port = 80
          }
          
          liveness_probe {
            http_get {
              path = "/"
              port = 80
            }
            initial_delay_seconds = 30
            period_seconds        = 10
          }
          
          readiness_probe {
            http_get {
              path = "/"
              port = 80
            }
            initial_delay_seconds = 15
            period_seconds        = 5
          }
        }
      }
    }
  }
}

# Front Service
resource "kubernetes_service" "front" {
  metadata {
    name      = "front"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }
  spec {
    selector = {
      app = kubernetes_deployment.front.spec[0].template[0].metadata[0].labels.app
    }
    port {
      port        = 80
      target_port = 80
    }
    type = "LoadBalancer"
  }
}

# Daemon Deployment
resource "kubernetes_deployment" "daemon" {
  metadata {
    name      = "daemon"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        app = "daemon"
      }
    }

    template {
      metadata {
        labels = {
          app = "daemon"
        }
      }

      spec {
        container {
          image = "${azurerm_container_registry.acr.login_server}/syki-daemon:latest"
          name  = "daemon"
          
          resources {
            limits = {
              cpu    = "0.5"
              memory = "512Mi"
            }
            requests = {
              cpu    = "0.2"
              memory = "256Mi"
            }
          }
          
          port {
            container_port = 8080
          }
          
          env {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = "Development"
          }
          
          env {
            name  = "Database__ConnectionString"
            value = "UserID=postgres;Password=${random_password.pg_password.result};Host=postgres;Port=5432;Database=syki-db;Pooling=true;Keepalive=60;"
          }
          
          liveness_probe {
            http_get {
              path = "/health"
              port = 8080
            }
            initial_delay_seconds = 30
            period_seconds        = 10
          }
          
          readiness_probe {
            http_get {
              path = "/health"
              port = 8080
            }
            initial_delay_seconds = 15
            period_seconds        = 5
          }
        }
      }
    }
  }
}

# Daemon Service
resource "kubernetes_service" "daemon" {
  metadata {
    name      = "daemon"
    namespace = kubernetes_namespace.syki.metadata[0].name
  }
  spec {
    selector = {
      app = kubernetes_deployment.daemon.spec[0].template[0].metadata[0].labels.app
    }
    port {
      port        = 80
      target_port = 8080
    }
  }
}

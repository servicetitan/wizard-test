# Get existing Kubernetes Ingress
data "kubernetes_ingress_v1" "ingress" {
  metadata {
    name      = var.ingress_name
    namespace = var.namespace
  }
}

# Create new A Record for Kubernetes Ingress
resource "azurerm_dns_a_record" "ingress_a" {
  name                = var.name
  zone_name           = var.dns_zone
  resource_group_name = var.resource_group_name
  ttl                 = 3600
  records             = [data.kubernetes_ingress_v1.ingress.status.0.load_balancer.0.ingress.0.ip]
}
# CREATE: Our own key vault. We aren't using the default key vault
# because that is meant to be reserved for infrastructure secrets.
module "kv" {
  source = "github.com/servicetitan/infra-iac//platform/modules/keyVault?ref=master"

  name                    = "${var.resource_prefix}-kv"
  resource_group_name     = var.resource_group_name
  resource_group_location = var.resource_group_location
  azurerm_subnet          = var.environment_details.vnet.subnets
  default_secrets         = var.default_secrets
}

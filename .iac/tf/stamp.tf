locals {
  environment_details_service      = data.terraform_remote_state.service_environment[0].outputs.details
}

resource "azurerm_key_vault_secret" "test_secret" {
  name         = "test-secret"
  value        = "test-secret-value"
  key_vault_id = local.environment_details_service.kv.id
}

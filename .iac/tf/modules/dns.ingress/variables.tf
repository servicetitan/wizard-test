variable "name" {
  type = string
}

variable "ingress_name" {
  type = string
}

variable "namespace" {
  type = string
}

variable "dns_zone" {
  type = string
}

variable "resource_group_name" { # This variable does not exist in corresponding bicep;
  type = string                  # adding it to retrieve existing DNS Zone resource
}

# variable "kube_config" { # Used to authx to k8s cluster in the corresponding bicep submodule;
#   type      = string     # will not use in this TF submodule, though, and will instead use in calling parent module per best practice
#   sensitive = true
# }
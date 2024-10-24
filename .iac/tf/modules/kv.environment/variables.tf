variable "resource_prefix" {
  type = string
}

variable "environment_details" {
  type = object({
    vnet = object({
      subnets = object({
        primary = object({
          name = string
          id   = string
        })
        pods = object({
          name = string
          id   = string
        })
        private = object({
          name = string
          id   = string
        })
      })
    })
  })
}

variable "resource_group_name" {
  type = string
}

variable "resource_group_location" {
  type = string
}

variable "default_secrets" {
  type = map(
    object({
      value        = string
      content_type = optional(string)
  }))
  description = "The default secrets to create in the key vault"
  default     = {}
}

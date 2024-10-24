# Assuming the following environment variables are set:
# $env:SUBSCRIPTIONS
# $env:ENVIRONMENTS
# $env:STAMPS
# $env:SERVICE_GROUP_NAME
# $env:SERVICE_GROUP_TEAM
# $env:SERVICE_NAME

# Test data toggle
$testDataOn = $false

if ($testDataOn) {
    $env:SUBSCRIPTIONS = '[{"name": "Infra sandbox", "subscriptionId": "eac7ab10-a55c-45ee-b87d-d27b337c7d40", "key": "sandbox", "workload": "production"}]'
    $env:ENVIRONMENTS = '[{"name": "cicddemo", "subscription": "sandbox"}]'
    $env:STAMPS = '[{"name": "wus202", "environments": ["cicddemo"]}, {"name": "wus201", "environments": ["cicddemo"]}]'
    $env:SERVICE_GROUP_NAME = "infrademo"
    $env:SERVICE_GROUP_TEAM = "infra"
    $env:SERVICE_NAME = "cicd"
}

# Convert JSON strings from environment variables to PowerShell objects
$subscriptions = $env:SUBSCRIPTIONS | ConvertFrom-Json
$environments = $env:ENVIRONMENTS | ConvertFrom-Json
$stamps = $env:STAMPS | ConvertFrom-Json
# Ensure we have arrays, otherwise yaml will be incorrect
if ($subscriptions -isnot [System.Collections.IEnumerable]) {
    $subscriptions = @($subscriptions)
}
if ($environments -isnot [System.Collections.IEnumerable]) {
    $environments = @($environments)
}
if ($stamps -isnot [System.Collections.IEnumerable]) {
    $stamps = @($stamps)
}

# Load the YAML file
$yamlFilePath = ".iac/environments.yaml"
$yamlContent = Get-Content -Raw -Path $yamlFilePath | ConvertFrom-Yaml

# Replace placeholders in the YAML content
$yamlContent.serviceGroup.name = $env:SERVICE_GROUP_NAME
$yamlContent.serviceGroup.team = $env:SERVICE_GROUP_TEAM
$yamlContent.service.name = $env:SERVICE_NAME
$yamlContent.serviceGroup.subscriptions = $subscriptions
$yamlContent.serviceGroup.environments = $environments
$yamlContent.serviceGroup.stamps = $stamps

# Write the modified content back to the YAML file
$yamlContent | ConvertTo-Yaml | Set-Content -Path $yamlFilePath

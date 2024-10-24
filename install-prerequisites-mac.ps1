$ErrorActionPreference = 'Stop'

# Install homebrew
which brew | Out-Null
if ($LASTEXITCODE -ne 0) {
    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
}
brew update

# Install .NET SDK
which dotnet | Out-Null
if ($LASTEXITCODE -ne 0) {
    brew install --cask dotnet-sdk@7
}

# Install pwsh
which pwsh | Out-Null
if ($LASTEXITCODE -ne 0) {
    brew install powershell/tap/powershell
}

# Switch to PWSH from here on out
if ($PSVersionTable.PSVersion.Major -lt 7) {
    return (pwsh $PSScriptRoot\install-prerequisites.ps1) 
}

# YAML parsing module used by many scripts
install-module powershell-yaml | Out-Null

# Azure CLI
which az | Out-Null
if ($LASTEXITCODE -ne 0) {
    brew update && brew install azure-cli
}

# AKS CLI tools (kubectl and kubelogin)
which kubectl | Out-Null
if ($LASTEXITCODE -ne 0) { brew install azure-cli }
which kubelogin | Out-Null
if ($LASTEXITCODE -ne 0) { brew install Azure/kubelogin/kubelogin }

# Okteto
which okteto | Out-Null
if ($LASTEXITCODE -ne 0) {
    brew install okteto
}
okteto context use https://okteto.westus2-01.dev.okteto.svc.st.dev/

# GH CLI
which gh | Out-Null
if ($LASTEXITCODE -ne 0) {
    brew install gh
}

# Service Titan iac tool
which iac | Out-Null
if ($LASTEXITCODE -ne 0) {
    Invoke-Expression (Invoke-RestMethod -Uri https://avkig4zhesa.blob.core.windows.net/packages/install.ps1)
}

$ErrorActionPreference = 'Stop'

# Install .NET SDK
where.exe dotnet | Out-Null
if ($LASTEXITCODE -ne 0) {
    winget install 'Microsoft.DotNet.SDK.8'
}

# Install pwsh
where.exe pwsh | Out-Null
if ($LASTEXITCODE -ne 0) {
    dotnet tool install --global PowerShell --version 7.3.4
}

# Switch to PWSH from here on out
if ($PSVersionTable.PSVersion.Major -lt 7) {
    return (pwsh $PSScriptRoot\install-prerequisites.ps1) 
}

# YAML parsing module used by many scripts
install-module powershell-yaml | Out-Null

# Azure CLI
where.exe az | Out-Null
if ($LASTEXITCODE -ne 0) {
    Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi
    Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'
    Remove-Item .\AzureCLI.msi
    # Add to path
    $env:PATH = "C:\Program Files (x86)\Microsoft SDKs\Azure\CLI2\wbin;$env:PATH"
}

# AKS CLI tools (kubectl and kubelogin)
where.exe kubectl | Out-Null
if ($LASTEXITCODE -ne 0) { az aks install-cli }
where.exe kubelogin | Out-Null
if ($LASTEXITCODE -ne 0) { az aks install-cli }

# Scoop (so we can get Okteto)
where.exe scoop | Out-Null
if ($LASTEXITCODE -ne 0) {
    Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
    Invoke-RestMethod get.scoop.sh | Invoke-Expression
}

# Okteto
where.exe okteto | Out-Null
if ($LASTEXITCODE -ne 0) {
    scoop install okteto
}
okteto context use https://okteto.westus2-01.dev.okteto.svc.st.dev/

# GH CLI
where.exe gh | Out-Null
if ($LASTEXITCODE -ne 0) {
    winget install --id GitHub.cli
}

# Service Titan iac tool
where.exe iac | Out-Null
if ($LASTEXITCODE -ne 0) {
    Invoke-Expression (Invoke-RestMethod -Uri https://avkig4zhesa.blob.core.windows.net/packages/install.ps1)
}
# FlagExplorer Backend API

.NET 8 Web API that serves country data and integrates with the REST Countries API.

## Prerequisites

- .NET 8
- Visual Studio 2022

  
## 🚀 Quick Start with PowerShell

To clone the repositories, install dependencies, and run both frontend and backend:

1. Open PowerShell
2. Run the script below or save it as `setup.ps1` and execute it:

```powershell
# Setup Script for FlagExplorer Frontend and Backend

Write-Host "Starting setup for FlagExplorer App..."

# === CONFIG ===
$frontendRepo = "https://github.com/OM-Assessment/FlagExplorerFrontend.git"
$backendRepo = "https://github.com/OM-Assessment/FlagExplorerBackend.git"
$frontendFolder = "FlagExplorerFrontend"
$backendFolder = "FlagExplorerBackend"

# === NODE & ANGULAR SETUP ===
if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
    Write-Host "Node.js is not installed. Please install it from https://nodejs.org/"
    exit 1
}

if (-not (Get-Command ng -ErrorAction SilentlyContinue)) {
    Write-Host "Angular CLI not found. Installing globally..."
    npm install -g @angular/cli
}

# === .NET CHECK ===
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Host ".NET SDK is not installed. Please install it."
    exit 1
}

# === CLONE FRONTEND ===
if (-not (Test-Path $frontendFolder)) {
    Write-Host "============ Cloning frontend repository ============"
    git clone $frontendRepo
} else {
    Write-Host "Frontend folder already exists. Skipping clone."
}

# === CLONE BACKEND ===
if (-not (Test-Path $backendFolder)) {
    Write-Host "============ Cloning backend repository ============"
    git clone $backendRepo
} else {
    Write-Host "Backend folder already exists. Skipping clone."
}

# === SETUP FRONTEND ===
Write-Host "Installing Angular frontend dependencies..."
Push-Location $frontendFolder
npm install
Start-Process powershell -ArgumentList "ng serve"
Pop-Location

# === SETUP BACKEND ===
Write-Host "============ Restoring and running .NET backend ============"
Push-Location $backendFolder
dotnet restore .\backend\FlagExplorerBackend\
dotnet build .\backend\FlagExplorerBackend\
dotnet run --project .\backend\FlagExplorerBackend
Pop-Location

Write-Host "Setup complete. Frontend should be available at http://localhost:4200 and backend at http://localhost:5000"
```

## Running Tests
   ```
   dotnet test .\backend\Tests\FlagExplorerBackend.Tests.Unit\
   dotnet test .\backend\Tests\FlagExplorerBackend.Tests.Integration\
   ```



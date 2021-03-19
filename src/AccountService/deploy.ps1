$project="AccountService"
$helmRelease="accounts-service"
$chart="accounts"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

Write-Host $ScriptDirectory

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
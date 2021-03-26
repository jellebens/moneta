$project="AccountService"
$helmRelease="accounts-service"
$chart="accounts"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
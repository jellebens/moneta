$project="TransactionService"
$helmRelease="transactions-service"
$chart="01.transactions"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

Write-Host $ScriptDirectory

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
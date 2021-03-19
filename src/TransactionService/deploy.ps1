$project="TransactionService"
$helmRelease="transaction-service"
$chart="transactionsvc"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

Write-Host $ScriptDirectory

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
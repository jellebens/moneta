$project="Moneta.Frontend.Web"
$helmRelease="frontend-web"
$chart="web"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

Write-Host $ScriptDirectory

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
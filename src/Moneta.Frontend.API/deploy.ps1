$project="Moneta.Frontend.API"
$helmRelease="frontend-api"
$chart="frontendapi"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
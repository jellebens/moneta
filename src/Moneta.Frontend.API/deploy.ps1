$project="Moneta.Frontend.API"
$helmRelease="frontend-api"
$chart="01.frontendapi"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
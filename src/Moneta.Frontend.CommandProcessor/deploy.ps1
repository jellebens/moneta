$project="Moneta.Frontend.CommandProcessor"
$helmRelease="frontend-commandprocessor"
$chart="frontend-commandprocessor"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
$project="Moneta.Frontend.CommandProcessor"
$helmRelease="frontend-commandprocessor"
$chart="01.commandprocessor"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
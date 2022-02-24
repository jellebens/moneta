$project="InstrumentService"
$helmRelease="instruments-service"
$chart="01.instruments"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
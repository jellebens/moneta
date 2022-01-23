$project="InstrumentService"
$helmRelease="instruments-service"
$chart="instruments"

$ScriptDirectory = Split-Path -Path $PSScriptRoot -Parent

. ("$ScriptDirectory\deploy.ps1")

deploy $project $chart $helmRelease
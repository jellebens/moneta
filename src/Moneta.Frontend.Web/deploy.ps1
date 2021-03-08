$project="Moneta.Frontend.Web"
$helmRelease="Frontend.Web"

$helmInstalls =  helm list -n moneta -o json | ConvertFrom-Json

$helm = $helmInstalls |  Where-Object {$_.name -eq $helmRelease }
$rev = ($helm.revision -as [int]) + 1
$appVersion = $helm.app_version

if([string]::IsNullOrEmpty($appVersion)){
    $appVersion = "0.1"
}else {
    $parts=$appVersion.Split('.');
    $appVersion = $parts[0] + "." + $parts[1]
}

$tag = "$appVersion.$rev";

Write-Host "Deploying frontend version $tag"

docker build -f "$project\Dockerfile" . -t "jellebens/moneta:$tag"

docker push "jellebens/moneta:$tag"

Write-Host "Helm upgrade"

$chartYaml = "$project\web\Chart.yaml"

$line = Get-Content $chartYaml | Select-String appVersion | Select-Object -ExpandProperty Line

$content = Get-Content $chartYaml 
$content | ForEach-Object {$_ -replace $line,"appVersion: $tag"} | Set-Content $chartYaml
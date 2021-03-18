
Function deploy($project, $helmRelease){    
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

    $repository="jellebens/" + $helmRelease
    $image= $repository + ":" + $tag

    docker build -f "$project\Dockerfile" . -t $image

    docker push $image

    Write-Host "Helm upgrade"

    $chartYaml = "$project\web\Chart.yaml"

    $line = Get-Content $chartYaml | Select-String appVersion | Select-Object -ExpandProperty Line

    $content = Get-Content $chartYaml 
    $content | ForEach-Object {$_ -replace $line,"appVersion: $tag"} | Set-Content $chartYaml

    return helm upgrade --install $helmRelease "$project\web" --set image.repository=$repository -n moneta
}

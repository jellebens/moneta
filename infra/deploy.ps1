
$DOCKER_REGISTRY_SERVER="docker.io"
$DOCKER_USER="jellebens"
$DOCKER_EMAIL="jellebens@outlook.com"
$PASSWORD=(Read-Host "Enter Password" -AsSecureString)

$DOCKER_PASSWORD=ConvertFrom-SecureString -SecureString $PASSWORD -AsPlainText

kubectl delete secret dockerhub -n moneta
kubectl create secret docker-registry dockerhub --docker-server=$DOCKER_REGISTRY_SERVER --docker-username=$DOCKER_USER --docker-password=$DOCKER_PASSWORD --docker-email=$DOCKER_EMAIL -n moneta
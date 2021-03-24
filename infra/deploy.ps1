
$DOCKER_REGISTRY_SERVER="docker.io"
$DOCKER_USER="jellebens"
$DOCKER_EMAIL="jellebens@outlook.com"
$PASSWORD=(Read-Host "Enter docker Password" -AsSecureString)

$DOCKER_PASSWORD=ConvertFrom-SecureString -SecureString $PASSWORD -AsPlainText

#kubectl delete secret dockerhub -n moneta
#kubectl create secret docker-registry dockerhub --docker-server=$DOCKER_REGISTRY_SERVER --docker-username=$DOCKER_USER --docker-password=$DOCKER_PASSWORD --docker-email=$DOCKER_EMAIL -n moneta

$APP_ID=780558495917932
#(https://developers.facebook.com/apps/452741245931933/settings/basic/)
$SECRET=(Read-Host "Enter Facebook AppSecret" -AsSecureString)
$APP_SECRET=ConvertFrom-SecureString -SecureString $SECRET -AsPlainText
kubectl delete secret facebook -n moneta
kubectl create secret generic facebook --from-literal=AppId=$APP_ID --from-literal=AppSecret=$APP_SECRET -n moneta





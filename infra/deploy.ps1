
$DOCKER_REGISTRY_SERVER="docker.io"
$DOCKER_USER="jellebens"
$DOCKER_EMAIL="jellebens@outlook.com"
$PASSWORD=(Read-Host "Enter docker Password" -AsSecureString)

$DOCKER_PASSWORD=ConvertFrom-SecureString -SecureString $PASSWORD -AsPlainText

#kubectl delete secret dockerhub -n moneta
#kubectl create secret docker-registry dockerhub --docker-server=$DOCKER_REGISTRY_SERVER --docker-username=$DOCKER_USER --docker-password=$DOCKER_PASSWORD --docker-email=$DOCKER_EMAIL -n moneta

$CLIENT_ID="c15b6bd2-3978-45e8-8fdb-f20cea05211c"
#(https://developers.facebook.com/apps/452741245931933/settings/basic/)
$SECRET=(Read-Host "Enter Microsoft ClientSecret" -AsSecureString)
$CLIENT_SECRET=ConvertFrom-SecureString -SecureString $SECRET -AsPlainText
#kubectl delete secret azure -n moneta
#kubectl create secret generic azure --from-literal=ClientId=$CLIENT_ID --from-literal=ClientSecret=$CLIENT_SECRET -n moneta





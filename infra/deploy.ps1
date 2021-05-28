
#  $DOCKER_REGISTRY_SERVER="docker.io"
#  $DOCKER_USER="jellebens"
#  $DOCKER_EMAIL="jellebens@outlook.com"
#  $PASSWORD=(Read-Host "Enter docker Password" -AsSecureString)

#  $DOCKER_PASSWORD=ConvertFrom-SecureString -SecureString $PASSWORD -AsPlainText

# kubectl delete secret dockerhub -n moneta
# kubectl create secret docker-registry dockerhub --docker-server=$DOCKER_REGISTRY_SERVER --docker-username=$DOCKER_USER --docker-password=$DOCKER_PASSWORD --docker-email=$DOCKER_EMAIL -n moneta

  $CLIENT_ID=""
  $TENANT_ID=""
#   $SECRET=(Read-Host "Enter Microsoft ClientSecret" -AsSecureString)
#   $CLIENT_SECRET=ConvertFrom-SecureString -SecureString $SECRET -AsPlainText
 kubectl delete secret frontend -n moneta
 kubectl create secret generic frontend --from-literal=ClientId=$CLIENT_ID --from-literal=TenantId=$TENANT_ID -n moneta

#  $SECRET=(Read-Host "Enter JWT Secret" -AsSecureString)
#  $JWT_SECRET=ConvertFrom-SecureString -SecureString $SECRET -AsPlainText
#  kubectl delete secret jwt -n moneta
#  kubectl create secret generic jwt --from-literal=Secret=$JWT_SECRET -n moneta

# $PASSWORD=(Read-Host "Enter Microsoft SQL SA password" -AsSecureString)
# $SA_PWD=ConvertFrom-SecureString -SecureString $PASSWORD -AsPlainText
# $AccountsDb="Data Source=tcp:mssql,1433;Initial Catalog=AccountsDb;User Id=sa;Password=$SA_PWD"
# $TransactionsDb="Data Source=tcp:mssql,1433;Initial Catalog=TransactionsDb;User Id=sa;Password=$SA_PWD"
# kubectl delete secret mssql -n moneta
# kubectl create secret generic mssql --from-literal=SA_PWD=$SA_PWD --from-literal=AccountsDb=$AccountsDb --from-literal=TransactionsDb=$TransactionsDb -n moneta
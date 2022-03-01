
#  $DOCKER_REGISTRY_SERVER="docker.io"
#  $DOCKER_USER="jellebens"
#  $DOCKER_EMAIL="jellebens@outlook.com"
#  $PASSWORD=(Read-Host "Enter docker Password" -AsSecureString)

#  $DOCKER_PASSWORD=ConvertFrom-SecureString -SecureString $PASSWORD -AsPlainText

# kubectl delete secret dockerhub -n moneta
# kubectl create secret docker-registry dockerhub --docker-server=$DOCKER_REGISTRY_SERVER --docker-username=$DOCKER_USER --docker-password=$DOCKER_PASSWORD --docker-email=$DOCKER_EMAIL -n moneta

#   $CLIENT_ID=""
#   $SECRET=(Read-Host "Enter Microsoft ClientSecret" -AsSecureString)
#   $CLIENT_SECRET=ConvertFrom-SecureString -SecureString $SECRET -AsPlainText
#  kubectl delete secret azure -n moneta
#  kubectl create secret generic azure --from-literal=ClientId=$CLIENT_ID --from-literal=ClientSecret=$CLIENT_SECRET -n moneta


# $PASSWORD=(Read-Host "Enter Microsoft SQL SA password" -AsSecureString)
# $SA_PWD=ConvertFrom-SecureString -SecureString $PASSWORD -AsPlainText
# $AccountsDb="Data Source=tcp:mssql,1433;Initial Catalog=AccountsDb;User Id=sa;Password=$SA_PWD"
# $TransactionsDb="Data Source=tcp:mssql,1433;Initial Catalog=TransactionsDb;User Id=sa;Password=$SA_PWD"
# $InstrumentsDb="Data Source=tcp:mssql,1433;Initial Catalog=InstrumentsDb;User Id=sa;Password=$SA_PWD"
# kubectl delete secret mssql -n moneta
# kubectl create secret generic mssql --from-literal=SA_PWD=$SA_PWD --from-literal=AccountsDb=$AccountsDb --from-literal=TransactionsDb=$TransactionsDb --from-literal=InstrumentsDb=$InstrumentsDb -n moneta

# $API_KEY=(Read-Host "Enter Microsoft ClientSecret" -AsSecureString)
# $API_SECRET=ConvertFrom-SecureString -SecureString $API_KEY -AsPlainText
#  kubectl delete secret api -n moneta
#  kubectl create secret generic api --from-literal=financeapi-key=$API_SECRET -n moneta

#$RABBITMQ_USER=
#$RABBITMQ_PASSWORD=
#kubectl create secret generic rabbitmq --from-literal=username=$RABBITMQ_USER --from-literal=password=$RABBITMQ_PASSWORD  -n moneta
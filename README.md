# Install Kafka using strimzi operator
helm install strimzi-operator strimzi/strimzi-kafka-operator -n strimzi-system --set watchNamespaces="{moneta,databus}"

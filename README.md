# Install Kafka using strimzi operator
helm repo add strimzi https://strimzi.io/charts/
helm install strimzi-operator strimzi/strimzi-kafka-operator -n strimzi-system --set watchNamespaces="{moneta}"

# Install cert manager
https://cert-manager.io/docs/installation/kubernetes/

 helm repo add jetstack https://charts.jetstack.io
 helm install cert-manager jetstack/cert-manager --namespace cert-manager --version v1.2.0 --create-namespace --set installCRDs=true

 # Modify Coredns
Confure local dns to resolve hostname to internal ip

jellebens.ddns.net.:53 {
    errors
    log
    prometheus :9153
    loadbalance
    forward . 192.168.1.41 192.168.1.43
    cache 30
}

# Run JAeger collector locally
docker run --name jaeger -p 13133:13133 -p 16686:16686 -p 4317:4317 -p 6831:6831/udp -d --restart=unless-stopped jaegertracing/opentelemetry-all-in-one
# Build images and publish to a cluster accessible repo
Locally this can be done with `..\BuildDevImages.ps1`

# Install the Orleans Custom Resource Definitions
```
kubectl apply -f crd.yaml
```

# Install a ingress controller with (Optional)
```
kubectl create namespace ingress-nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/static/mandatory.yaml
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/static/provider/cloud-generic.yaml
```

# Create namespace for Orleans Cluster
```
kubectl create namespace tickets-dev
```

# Install Azurite for storage emulation (Dev Only)
```
kubectl apply -f azurite.yaml
```

# Install the Cluster

```
kubectl apply -k orleans/dev
```

# Access the dashboard and api pages
e.g. http://ticketapi.localtest.me/swagger/index.html and http://dashboard.localtest.me/

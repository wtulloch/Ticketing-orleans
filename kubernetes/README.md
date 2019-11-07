# Build images and publish to a cluster accessible repo
Locally this can be done with `..\BuildDevImages.ps1`

# Install the Orleans Custom Resource Definitions
```
kubectl apply -f crd.yaml
```


# Install a ingress controller with (Optional)

Note: If you are on a windows machine and have IIS installed and running you will need to stop it first. From cmd or Powershell - as administrator -  use the following: ```iisreset /stop```.

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

# Accessing the Orleans dashboard, API Swagger documentation and the test Ticketing app

To access the Swagger documentation for the Ticket API use the following link: http://ticketapi.localtest.me/

To access the Orleans dashboard use the following: http://dashboard.localtest.me/.

The test ticketing application can be accessed at http://ticketweb.localtest.me/

# Updating the number of Orleans silos running

There two ways of increasing the number of silos running.

1. In the silo-deployment.yaml, under spec increase the number of replicas.
2. To dynamically increase or decrease the number of silos, from the command line using the following ```kubectl scale deployment -n tickets-dev ticketsilo-dev --replicas=4``` where the value assigned to --replicas is the number of instances you want.
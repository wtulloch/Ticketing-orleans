$ScriptDir = Split-Path -parent $MyInvocation.MyCommand.Path

docker build -t ticketsilo:dev -f $ScriptDir/ticketing-server/Silo/Dockerfile $ScriptDir/
docker build -t ticketapi:dev -f $ScriptDir/ticketing-server/TicketingApi/Dockerfile $ScriptDir/

# kubectl create namespace tickets
# kubectl run ticketsilo --image=ticketsilo:dev --namespace=tickets --image-pull-policy=Never
# kubectl run ticketapi --image=ticketapi:dev --namespace=tickets --image-pull-policy=Never
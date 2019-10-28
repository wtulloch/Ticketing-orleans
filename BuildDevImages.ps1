$ScriptDir = Split-Path -parent $MyInvocation.MyCommand.Path

docker build -t ticketsilo:dev -f $ScriptDir/ticketing-server/Silo/Dockerfile $ScriptDir/
docker build -t ticketapi:dev -f $ScriptDir/ticketing-server/TicketingApi/Dockerfile $ScriptDir/
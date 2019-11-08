# Orleans ticketing example with Kubernetes

This is a simple sample project demonstrating a ticket booking process using MS Orleans which has been set-up in Kubernetes to allow scaling out the silo hosts.

## Key projects

### Ticketing-orleans

In the ticketing-server folder you will ticketing-orleans solution. This contains the Silo project, the Ticketing API which is acting as the Orleans client and in the solution folder ```Grains``` the class libraries for the grain interfaces, grain implementations and data models.

### Ticketing Sample App

This is a simple web application which allows the user to select a seat for a given show which then calls the ticketing api to book it. 

### Kubernetes

In the Kubernetes folder is all the magic for hosting the projects in kubernetes. For further information on getting it up and running, read the README in the folder.

## About the project
The first purpose of this project was to explore creating a distributed system for managing ticket sales in an online booking system as an alternative to an existing system that was using in memory concurrent queues and dictionaries for the purpose.

The second purpose was demonstrate hosting MS Orleans within Kubernetes.

The project is only a simplified version of the existing system since it was written purely to validate an idea.


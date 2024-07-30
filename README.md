# Centralized Logging System
## Overview
This project implements a centralized logging system using .NET Core, Serilog, RabbitMQ, and Elasticsearch Kibbana . It consists of two main components:

## Publisher:
### Configures Serilog in .NET Core applications to send logs to RabbitMQ queues.
## Consumer:
### A background service that consumes logs from RabbitMQ queues, writes them to files, and inserts them into Elasticsearch indices.

## Architecture
![impl3](https://github.com/khaledibrahim1015/Queuing-Logger/assets/91853322/cb5d5e98-e68f-4e90-9ba0-8b007f54cd14)


Prerequisites
- .NET Core 3.1 or later
- RabbitMQ server
- Elasticsearch server

## Publisher Component
Installation
Add the following NuGet packages to your .NET Core application:
``` 
dotnet add package Serilog
dotnet add package Serilog.Sinks.RabbitMQ
dotnet add package RabbitMQ.Client
```

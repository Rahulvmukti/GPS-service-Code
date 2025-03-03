Code for GPS Service
# GPS Coordinates Update Microservice

## Overview
This microservice is built using ASP.NET and updates GPS coordinates in an SQL Server database every 10 seconds. The service can fetch GPS coordinates from a specified source and store them in the database for further processing or monitoring.

## Features
- Fetches GPS coordinates every 10 seconds.
- Updates SQL Server database with the latest GPS coordinates.
- Uses ASP.NET Core's `IHostedService` for background task execution.
- Configurable to fetch coordinates from an external API or any other source.

## Prerequisites
To run this application, make sure the following are installed on your machine:
- .NET 4.5 or later
- MS SQL Server (or any compatible database)
- Visual Studio or any compatible IDE

## Installation

1. Clone the repository
2. add in service using below command
   "C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe" "service path"
3. add connection string in app.config file
4. Start service
5. Set with Automated

#!/bin/sh

dotnet run --project tests/MultipleLogger/MultipleLogger.csproj
netstat | grep kul
# netstat | grep $(wk-ip-address)
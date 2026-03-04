@echo off
setlocal

set "ASPNETCORE_ENVIRONMENT=Production"
set "APPHOST_MODE=Production"

dotnet run --project AppHost\AppHost.csproj

endlocal

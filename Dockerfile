FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim

WORKDIR /app
COPY . .

CMD ["dotnet", "FileCounter.dll"]
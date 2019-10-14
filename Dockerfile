FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim

WORKDIR /app
COPY . .

RUN set -ex; \
    apt-get update && apt-get install -y unzip; \
	mkdir /vsdbg; \
	curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

CMD ["dotnet", "FileCounter.dll"]
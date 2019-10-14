dotnet publish -c Debug -f netcoreapp3.0 -o bin/output
docker build -t quadient.azurecr.io/cloud/filecounter:demo -f .\Dockerfile bin/output
docker push quadient.azurecr.io/cloud/filecounter:demo
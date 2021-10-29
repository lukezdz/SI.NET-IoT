FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY bin/Release/net5.0/ Api/
WORKDIR /Api

ENTRYPOINT ["dotnet", "DataExplorer.dll"]

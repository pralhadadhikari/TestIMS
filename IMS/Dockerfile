FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["IMS.web/IMS.web.csproj", "IMS.web/"]
COPY ["IMS.Infrastructure/IMS.Infrastructure.csproj", "IMS.Infrastructure/"]
COPY ["IMS.Models/IMS.Models.csproj", "IMS.Models/"]
COPY ["IMS.Utility/IMS.Utility.csproj", "IMS.Utility/"]

RUN dotnet restore "IMS.web/IMS.web.csproj"

COPY . .

RUN dotnet publish "IMS.web/IMS.web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "IMS.web.dll"]
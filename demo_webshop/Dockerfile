#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["demo_webshop.csproj", "demo_webshop/"]
RUN dotnet restore "demo_webshop/demo_webshop.csproj"

WORKDIR "/src/demo_webshop"
COPY . .

RUN dotnet build "demo_webshop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "demo_webshop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "demo_webshop.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet demo_webshop.dll
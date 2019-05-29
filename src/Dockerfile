FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY IdentityUsers/IdentityUsers.sln .
COPY IdentityUsers/IdentityUsers.csproj .
RUN dotnet restore

# copy everything else and build app
COPY IdentityUsers/. ./src/
WORKDIR /app/src
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/src/out ./
ENTRYPOINT ["dotnet", "IdentityUsers.dll"]
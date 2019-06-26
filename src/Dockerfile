FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY IdentityUsers/IdentityUsers.csproj ./IdentityUsers/
RUN dotnet restore IdentityUsers/IdentityUsers.csproj

# copy everything else and build app
COPY IdentityUsers/. ./IdentityUsers/
WORKDIR /app/IdentityUsers
RUN dotnet publish IdentityUsers.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/IdentityUsers/out ./
ENTRYPOINT ["dotnet", "IdentityUsers.dll"]
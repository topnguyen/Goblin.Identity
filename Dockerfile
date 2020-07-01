# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .

COPY Goblin.Core/Goblin.Core/*.csproj ./Goblin.Core/Goblin.Core/
COPY Goblin.Core/Goblin.Core.Web/*.csproj ./Goblin.Core/Goblin.Core.Web/

COPY src/Cross/Goblin.Identity.Core/*.csproj ./src/Cross/Goblin.Identity.Core/
COPY src/Cross/Goblin.Identity.Mapper/*.csproj ./src/Cross/Goblin.Identity.Mapper/
COPY src/Cross/Goblin.Identity.Share/*.csproj ./src/Cross/Goblin.Identity.Share/

COPY src/Repository/Goblin.Identity.Contract.Repository/*.csproj ./src/Repository/Goblin.Identity.Contract.Repository/
COPY src/Repository/Goblin.Identity.Repository/*.csproj ./src/Repository/Goblin.Identity.Repository/

COPY src/Service/Goblin.Identity.Contract.Service/*.csproj ./src/Service/Goblin.Identity.Contract.Service/
COPY src/Service/Goblin.Identity.Service/*.csproj ./src/Service/Goblin.Identity.Service/

COPY src/Web/Goblin.Identity/*.csproj ./src/Web/Goblin.Identity/

RUN dotnet restore

# copy everything else and build app

COPY Goblin.Core/Goblin.Core/. ./Goblin.Core/Goblin.Core/
COPY Goblin.Core/Goblin.Core.Web/. ./Goblin.Core/Goblin.Core.Web/

COPY src/Cross/Goblin.Identity.Core/. ./src/Cross/Goblin.Identity.Core/
COPY src/Cross/Goblin.Identity.Mapper/. ./src/Cross/Goblin.Identity.Mapper/
COPY src/Cross/Goblin.Identity.Share/. ./src/Cross/Goblin.Identity.Share/

COPY src/Repository/Goblin.Identity.Contract.Repository/. ./src/Repository/Goblin.Identity.Contract.Repository/
COPY src/Repository/Goblin.Identity.Repository/. ./src/Repository/Goblin.Identity.Repository/

COPY src/Service/Goblin.Identity.Contract.Service/. ./src/Service/Goblin.Identity.Contract.Service/
COPY src/Service/Goblin.Identity.Service/. ./src/Service/Goblin.Identity.Service/

COPY src/Web/Goblin.Identity/. ./src/Web/Goblin.Identity/

WORKDIR /source
RUN dotnet publish -c release -o /publish --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /publish
COPY --from=build /publish ./
ENTRYPOINT ["dotnet", "Goblin.Identity.dll"]
# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY src .
RUN dotnet restore

# copy everything else and build app
COPY src .
WORKDIR /source
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS runtime
# Add bash
RUN apk update && apk add bash
# Add curl
RUN apk --no-cache add curl
# Copy the image built in previously
WORKDIR /app
COPY --from=build /app ./
USER 1001
ENTRYPOINT ["dotnet", "dotnet-helloworld.dll"]
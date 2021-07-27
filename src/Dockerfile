# base image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

# setup
WORKDIR /app
EXPOSE 9003

# tools
RUN apt-get update \
    && apt-get install -y software-properties-common vim \
    && apt-get update -y \
    && apt-get install -y build-essential \
    && apt-get install -y curl

# sdk image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# deploy
WORKDIR /src
COPY ["Rhino.Local/Rhino.Local.csproj", "Rhino.Local/"]
RUN dotnet restore "Rhino.Local/Rhino.Local.csproj"
COPY . .
WORKDIR "/src/Rhino.Local"
RUN dotnet build "Rhino.Local.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "Rhino.Local.csproj" -c Release -o /app/publish

# copy
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# variables
ENV RHINO_HOME=https://localhost:9001

# run
CMD dotnet Rhino.Local.dll $RHINO_HOME
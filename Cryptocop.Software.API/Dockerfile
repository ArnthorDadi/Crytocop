FROM mcr.microsoft.com/dotnet/core/sdk:3.1

LABEL maintainer="Arnthor Dadi Jonasson"

ENV ASPNETCORE_URLS=http://+:7000

COPY . /app
WORKDIR /app/Cryptocop.Software.API
RUN dotnet restore

WORKDIR ../Cryptocop.Software.API.Models
RUN dotnet restore

WORKDIR ../Cryptocop.Software.API.Repositories
RUN dotnet restore

WORKDIR ../Cryptocop.Software.API.Services
RUN dotnet restore

WORKDIR ../Cryptocop.Software.API

ENTRYPOINT [ "dotnet", "run" ]
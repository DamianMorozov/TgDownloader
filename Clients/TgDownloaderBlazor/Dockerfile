# See https://aka.ms/customizecontainer to learn how to customize your debug container and 
# how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Docker base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Image setup
WORKDIR /app
EXPOSE 7682/tcp
ENV ASPNETCORE_URLS=http://0.0.0.0:7682
USER app

# App
COPY "/app" "/app"
ENTRYPOINT ["dotnet", "TgDownloaderBlazor.dll"]

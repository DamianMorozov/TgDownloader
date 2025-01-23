# Guide to setup the Docker version

## Docker compose usage
```
docker pull damianmorozov/tgdownloader-console:latest
d: && cd d:\Dockers\tgdownloader-console
docker-compose down tgdownloader-console
docker-compose up -d tgdownloader-console
```

## File docker-compose.yml
```
services:
  tgdownloader-console:
    image: damianmorozov/tgdownloader-console:latest
    ports:
     - "7681:7681"
    environment:
     - TZ=Europe/Rome
    volumes:
     - .\TgStorage.db:/app/TgStorage.db:rw # optional
     - .\TgDownloader.xml:/app/TgDownloader.xml:rw # optional
     - .\TgDownloader.session:/app/TgDownloader.session:rw # optional
    container_name: tgdownloader-console
    restart: on-failure
```

## Using in a web browser
http://localhost:7681
```
dotnet TgDownloaderConsole.dll
```

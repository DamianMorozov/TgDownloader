# Руководство по настройке докер версии

## Docker compose usage
```
docker pull damianmorozov/tgdownloader-console:latest
d: && cd d:\Dockers\tgdownloader-console
docker-compose down tgdownloader-console
docker-compose up -d tgdownloader-console
```

## Файл d:\Dockers\tgdownloader-console\docker-compose.yml
```
services:
  tgdownloader-console:
    image: damianmorozov/tgdownloader-console:latest
    ports:
     - "7681:7681"
    environment:
     - TZ=Europe/Rome
    volumes:
     - d:\DATABASES\SQLITE\TgStorage.db:/app/TgStorage.db:rw # optional
     - d:\DATABASES\SQLITE\TgDownloader.xml:/app/TgDownloader.xml:rw # optional
     - .\TgDownloader.session:/app/TgDownloader.session:rw # optional
    container_name: tgdownloader-console
    restart: on-failure
```

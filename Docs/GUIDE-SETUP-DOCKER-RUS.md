# Руководство по настройке докер версии

- Скачать образ:								`docker pull frezeen/tgdownloader`
- Запустить контейнер в1:						`docker run -v <Folder>:/mnt/SQLITE --name tgdownloader_console -d frezeen/tgdownloader`
- Или Запустить контейнер в2:					`docker run --mount type=bind,source=<Folder>\,target=/mnt/SQLITE,bind-propagation=shared --name tgdownloader_console -d frezeen/tgdownloader`
- Выполнить баш:								`docker exec -it tgdownloader_console /bin/bash`
- Смотреть смонтированный каталог:				`ls -lh /mnt/SQLITE`
- Скопировать конфиг на хост:					`cp /root/TgDownloaderConsole-AnyCPU/TgDownloader.xml /mnt/SQLITE`
- Править конфиг на хосте:						`<Folder>\TgDownloader.xml`
- Скопировать конфиг в контейнер:				`cp /mnt/SQLITE/TgDownloader.xml /root/TgDownloaderConsole-AnyCPU/ -f`
- Скопировать сессию в контейнер (если есть):	`cp /mnt/SQLITE/TgDownloader.session /root/TgDownloaderConsole-AnyCPU -f`
- Просмотреть файлы в контейнере:				`ls -lh /root/TgDownloaderConsole-AnyCPU`
- Запусить программу:							`./run.sh`
- Остановить контейнер:							`docker stop tgdownloader_console`
- Запустить контейнер:							`docker start tgdownloader_console`

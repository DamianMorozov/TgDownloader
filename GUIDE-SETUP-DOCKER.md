# Guide to setup the docker version

- Get image:									`docker pull frezeen/tgdownloader`
- Run container v1:								`docker run -v <Folder>:/mnt/SQLITE --name tgdownloader_console -d frezeen/tgdownloader`
- Or run container v2							`docker run --mount type=bind,source=<Folder>\,target=/mnt/SQLITE,bind-propagation=shared --name tgdownloader_console -d frezeen/tgdownloader`
- Exec bash										`docker exec -it tgdownloader_console /bin/bash`
- View mount folder								`ls -lh /mnt/SQLITE`
- Copy config into the host						`cp /root/TgDownloaderConsole-AnyCPU/TgDownloader.xml /mnt/SQLITE`
- Edit config on the host						`<Folder>\TgDownloader.xml`
- Copy config into the container				`cp /mnt/SQLITE/TgDownloader.xml /root/TgDownloaderConsole-AnyCPU/ -f`
- Copy session into the container (if exists)	`cp /mnt/SQLITE/TgDownloader.session /root/TgDownloaderConsole-AnyCPU -f`
- View files in the container					`ls -lh /root/TgDownloaderConsole-AnyCPU`
- Run program									`./run.sh`
- Stop container								`docker stop tgdownloader_console`
- Start container								`docker start tgdownloader_console`

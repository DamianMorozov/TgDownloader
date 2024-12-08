## Guides
- [Guide to setup the desktop version](GUIDE-SETUP-DESKTOP.md)
- [Guide to setup the docker version](GUIDE-SETUP-DOCKER.md)

## Запуск TgDownloaderDesktop под ОС Windows
- Run the `TgDownloaderDesktop-Installing.ps1` file in PowerShell.

## Run TgDownloaderConsole under Windows OS
- Extract the `TgDownloader v1.2.333.zip` archive to `TgDownloader v1.2.333` directory
- Change the current directory to `cd TgDownloader v1.2.333`
- Run the `TgDownloaderConsole.exe` command or double click the file

## Run TgDownloaderConsole under Linux OS
- Extract the `TgDownloader v1.2.333.zip` archive to `TgDownloader v1.2.333` directory
- Change the current directory to `cd TgDownloader v1.2.333`
- Run the `dotnet TgDownloaderConsole.dll` command

## Quick Start
- Change app settings
- Set up storage
- Set up the Telegram client
- Set up a download

## Default settings for TgDownloaderConsole
- Delete the file `TgDownloader.xml`

# Guide for downloading groups
1. Join a group on Telegram.
2. Right click any message -> `Copy Message Link`.
3. Edit the link on the clipboard `https://t.me/c/group_id/message_number` like this `group_id`.
4. Run `TgDownloaderConsole.exe` (Windows) or `dotnet TgDownloaderConsole.dll` (Linux).
5. Go to the -> `Download settings` -> `Setup source (ID/username)` -> paste `group_id`.
6. Go to the -> `Setup download folder` -> `Drive:\Storage_path`.
7. Go to the -> `Manual download`.
8. Enjoy.

# Guide for filters settings
View filters -- list of all filters
Add filter -- add new filter
Edit filter -- edit exists filter
Remove filter -- remove exists filter
Reset filter -- remove all exists filters

## Example of add filter type `Single name`
Set name: develop
Set mask: c*#

## Example of add filter type `Single extension`
Set name: book
Set mask: pdf

## Example of add filter type `Multi name`
Set name: develops
Set mask: c*#, python

## Example of add filter type `Multi extension`
Set name: books
Set mask: epub, fb2, pdf

## Example of add filter type `File minimum size`
Set name: min 1 mb
Set file size type: MBytes
File minimum size: 1

## Example of add filter type `File minimum size`
Set name: max 2 gb
Set file size type: GBytes
File minimum size: 2

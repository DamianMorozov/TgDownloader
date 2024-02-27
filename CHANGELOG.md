# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
- Help menu
- Russian localization

## [0.4.30] - 2024-02-27
### Added to TgEfCore
- Application table support
- Filter table support
- Proxy table support
### Added to TgDownloaderBlazor
- Homepage
- Application section
- Filter section
- Proxy section

## [0.4.10] - 2024-02-25
- Added TgDownloaderBlazor app (web app)
- Added TgEfCore library (EF Core storage access library)
- Added TgEfCoreTests (EF Core storage access tests)

## [0.3.110] - 2024-02-17
### Added in TgDownloaderConsole
- Progressbar for downloading current file
- Mark all messages as read
### Added in TgDownloaderWinDesktop
- Progressbar for downloading current file
- Mark all messages as read
### Changed in TgDownloaderWinDesktop
- UI

## [0.3.20] - 2023-12-17
### Added
- Guide to setup the docker version

## [0.3.10] - 2023-12-09
### Changed
- NET 8 version updated
- Updated version of NuGet packages
### Added in TgDownloaderWinDesktop
- Copying fields in the source

## [0.2.580] - 2023-11-10
### Added
- [Issue template](ISSUE.md)

## [0.2.570] - 2023-11-09
### Added in TgDownloaderWinDesktop
- Auto update progress when downloading to the source page
### Fixed in TgDownloaderConsole
- Downloading a source that is not yet in the source table

## [0.2.550] - 2023-11-05
### Fixed in TgDownloaderConsole
- Reading the number of the last message
### Fixed in TgDownloaderWinDesktop
- Clearing the application table
- Message when client connects to Telegram server
- Correct loading of empty sources on first download
- Reading sources from Telegram
- Channel/dialogue scanning
### Added
- Guide to setting up the desktop version
- Channel/dialogue scanning

## [0.2.490] - 2023-11-02
### Fixed
- Creating a new storage

## [0.2.480] - 2023-11-01
### Added in TgDownloaderWinDesktop
- Add new proxy
- Edit proxy
- Return to proxies section
- Return to sources section
### Fixed in TgDownloaderWinDesktop
- Delete proxy

## [0.2.460] - 2023-10-29
### Fixed in TgDownloaderConsole
- Refactoring and tests
### Added in TgDownloaderConsole
- The progress of the download in the console title
### Fixed in TgDownloaderWinDesktop
- Refactoring and tests
### Added in TgDownloaderWinDesktop
- Connecting a client via proxy
- Disconnecting a client via proxy
- Saving settings
- Navigation to the source item page
### Fixed in TgStorage
- Fixed methods in Repositories

## [0.2.300] - 2023-06-27
### Fixed in TgDownloaderConsole
- Fixed errors with file TgDownloader.session
### Fixed in Tests
- Fixed errors in tests
### Added in TgDownloaderWinDesktop
- Edit app settings
- View/edit client settings
- Client
    - Connect/Disconnect
    - Hide password
    - State and exception view
- View proxies
- View sources and download
    - Check client ready
    - Load from Storage
    - Load from Telegram
    - Clear view
    - State and exception view
    - Download media

## [0.2.230] - 2023-06-13
### Fixed in TgDownloaderConsole
- Restore ApiId when session was deleted
- Auto-update after configuring the download directory
### Added
- TgDownloaderWinDesktop project (WPF UI - Fluent Navigation (MVVM | DI))
### Added in TgDownloaderConsole
- Menu Advanced -> Auto view events
- Auto-update last message ID at Advanced -> Auto view events

## [0.2.160] - 2023-04-27
### Changed
- Storage version 18
- Viewing sources in the storage
- Scan my chats / Scan my dialogs
- Projects structure and properties
### Added
- Date time field for source table
- GitHub actions

## [0.2.130] - 2023-04-20
### Changed
- New ORM-framework for SQLite storage (DevExpress XPO)
- Storage version 17
### Added
- Scanning channels/dialogs with the ability to save as sources
- Viewing sources in the storage with the ability to go to the download menu
- Store messages
### Deprecated
- Software v0.2.xxx has a new storage format, save the previous file, it will be overwritten

## [0.1.730] - 2023-03-12
### Added
- Filters settings
- Creating backup storage

## [0.1.630] - 2023-02-24
### Fixed
- Proxy for downloads
- Overwrite zero size files
### Added
- App setting for the session file
- App setting for the storage file
- App setting for the usage proxy
- Automatic directory creation for manual download
- Automatic directory creation for auto download
- Storage versions table

## [0.1.500] - 2023-01-31
### Added
- Proxy for downloads
- Client and proxy exception messages

## [0.1.430] - 2023-01-17
### Added
- Auto download

## [0.1.390] - 2023-01-10
### Added
- Set file date and time
- Scanning subdirectories for downloaded files to move them to the root directory
### Changed
- Combining source ID and user name settings

## [0.1.360] - 2023-01-06
### Fixed
- Entering a source ID
- Autosave and autoload settings to download a channel/group

## [0.1.350] - 2023-01-05
### Added
- Auto calculation of the start message identifier
- Manual set start message identifier
- Auto renaming downloaded files if the option to add an identifier to the file name is enabled
- Autosave and autoload the directory to download a channel/group
### Changed
- Switch method for choice boolean answer
### Fixed
- Rewriting messages

## [0.1.310] - 2023-01-02
### Added
- Message identifier in the download settings
- Saving application settings to an xml file
### Changed
- Setup downloads by channel/group identifier

## [0.1.250] - 2022-12-21
### Added
- Storage settings
- Skip downloaded files
- Autosave connection info at local storage file
### Changed
- Client settings
- Download settings

## [0.1.180] - 2022-12-13
### Added
- Info sub menu
- Download progress
### Changed
- Client menu
- Download menu
- Collect info
- Try catch exceptions

## [0.1.150] - 2022-12-10
### Added
- First release
- Menu
- Log
- Client
- Download

## [0.1.100] - 2022-12-08
### Added
- English localisation
- Tests

## [0.1.020] - 2022-12-07
### Added
- Git base files
- TgDownloaderConsole project

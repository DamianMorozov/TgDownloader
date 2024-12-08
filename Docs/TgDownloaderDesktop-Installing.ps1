Clear-Host
Write-Host "TgDownloaderDesktop PowerShell script: installing latest release"

$releasePath = "C:\TgDownloader-Releases"
$archivePath = "$releasePath\Archives"
$destinationPath = "$releasePath\TgDownloaderDesktop-AppPackages"

Write-Host "Directory creation"
New-Item -ItemType Directory -Force -Path $archivePath
New-Item -ItemType Directory -Force -Path $destinationPath

Write-Host "Searching for the latest release on GitHub via API"
$repoOwner = "DamianMorozov"
$repoName = "TgDownloader"
$apiUrl = "https://api.github.com/repos/$repoOwner/$repoName/releases/latest"

Write-Host "Getting information about the latest release"
$releaseInfo = Invoke-RestMethod -Uri $apiUrl -Headers @{ "User-Agent" = "PowerShell" }

if ($releaseInfo -and $releaseInfo.assets.Count -gt 0) {
    Write-Host "Creating a catalog for an archive with a tag"
    $tagName = $releaseInfo.tag_name -replace '[\/:*?"<>|]', '_'
    $tagFolderPath = "$archivePath\$tagName"
    New-Item -ItemType Directory -Force -Path $tagFolderPath

    Write-Host "Downloading assets for release: $($releaseInfo.tag_name)"
    foreach ($asset in $releaseInfo.assets) {
        if ($asset.name -like "*AppPackages.zip*") {
			Write-Host "Getting URL and file name: $apiUrl"
            $assetUrl = $asset.browser_download_url
            $fileName = $asset.name
            $filePath = "$tagFolderPath\$fileName"
            # Checking the existence of a file before downloading
            if (-not (Test-Path $filePath)) {
                try {
                    Write-Host "Checking the existence of a ${fileName}: downloading"
                    Invoke-WebRequest -Uri $assetUrl -OutFile $filePath
                    Write-Host "Unzipping the archive: $fileName"
                    Expand-Archive -Path $filePath -DestinationPath $destinationPath -Force
                } catch {
                    Write-Host "Error downloading or unzipping the file: $_"
                }
            } else {
                Write-Host "Checking the existence of a ${fileName}: already exists, skipping download"
            }
        }
    }

	# Checking Windows Developer Mode
    try {
        $devModeKeyPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock"
        $devModeValue = (Get-ItemProperty -Path $devModeKeyPath -Name AllowDevelopmentWithoutDevLicense -ErrorAction SilentlyContinue).AllowDevelopmentWithoutDevLicense
        if ($devModeValue -ne 1) {
            Write-Host "Checking Windows Developer Mode: is not enabled, please enable it to continue"
            exit
        } else {
            Write-Host "Checking Windows Developer Mode: is enabled"
        }
    } catch {
        Write-Host "Error checking Developer Mode: $_"
    }

    Write-Host "Searching for the last release"
    try {
        $latestFolder = Get-ChildItem -Path $destinationPath | Sort-Object Name | Select-Object -Last 1

        if ($latestFolder) {
            $installScript = "$($latestFolder.FullName)\Install.ps1"
            if (Test-Path $installScript) {
                Write-Host "Starting the software installation: Install.ps1 is found"
                & $installScript
            } else {
                Write-Host "Starting the software installation: Install.ps1 is not found in the catalog $($latestFolder.FullName)"
            }
        } else {
            Write-Host "No folders found in the destination path."
        }
    } catch {
        Write-Host "Error searching for the last release folder: $_"
    }

    Write-Host "Installation of the release has been completed: $($releaseInfo.tag_name)"
} else {
    Write-Host "Couldn't find the latest release or no assets available."
}
@echo off
cls
setlocal
set "sourceDir=..\..\..\TgDownloader-Helpers\"
set "destDir=.\"
set "fileSession=TgDownloader.session"
set "fileXml=TgDownloader.xml"

echo ------------------------------------------------------------
echo ---          TgDownloaderConsole/Post-build.cmd          ---
echo ------------------------------------------------------------
echo [ ] Start job
call :job "%sourceDir%%fileSession%"
call :job "%sourceDir%%fileXml%"
goto end

:job
echo [ ] Check file exists: "%~1"
if exist "%~1" (
	xcopy "%~1" "%destDir%" /Y /S /Q /F /R /V >nul
	echo [v] File exists and was updated from source
) else (
    echo [!] File does not exist
)
goto :eof

:end
echo [ ] End job
endlocal

:exit
echo [ ] Exit from console

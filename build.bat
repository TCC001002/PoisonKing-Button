@echo off
setlocal
cd /d "%~dp0"

set "CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
if not exist "%CSC%" goto NOCSC

if not exist build mkdir build
taskkill /f /im WinProfile.exe >nul 2>&1
if exist build\WinProfile.exe del /f /q build\WinProfile.exe

echo Compiling dev build: src\*.cs -^> build\WinProfile.exe ...
set "ICON="
if exist src\app.ico set "ICON=/win32icon:src\app.ico"
"%CSC%" /nologo /optimize+ /codepage:65001 %ICON% /win32manifest:src\app.manifest /resource:src\banner.jpg,kpres.banner /resource:src\app.ico,kpres.icon /target:winexe /out:build\WinProfile.exe /r:System.dll /r:System.Core.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll src\Program.cs src\ProfileStore.cs src\MainForm.cs src\AssemblyInfo.cs > build\build-log.txt 2>&1

if exist build\WinProfile.exe goto OK

echo.
echo [FAILED] Compilation error, see build\build-log.txt
echo ------------------------------------------
type build\build-log.txt
echo ------------------------------------------
echo.
pause
exit /b 1

:OK
echo.
echo [OK] build\WinProfile.exe generated.
del /f /q build\build-log.txt >nul 2>&1
echo.
pause
exit /b 0

:NOCSC
echo [ERROR] .NET Framework compiler not found on this system.
pause
exit /b 1

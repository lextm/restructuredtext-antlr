@echo off

for /f "usebackq tokens=*" %%i in (`vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set InstallDir=%%i
)

if exist "%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe" (
  "%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe" %*
)
@echo on
if exist "%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe" (
  set msBuildExe="%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe"
)

call %msBuildExe% ReStructuredText.sln /t:restore /p:Configuration=RELEASE /m:1
call %msBuildExe% ReStructuredText.sln /t:clean /p:Configuration=RELEASE /m:1
call %msBuildExe% ReStructuredText.sln /p:Configuration=RELEASE /m:1

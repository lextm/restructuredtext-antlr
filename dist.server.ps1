Remove-Item -Recurse -Force output
Set-Location Server
dotnet publish --self-contained -r win-x86 -c release -o ..\output\win32_x86 -p:PublishSingleFile=true
dotnet publish --self-contained -r win-x64 -c release -o ..\output\win32_x64 -p:PublishSingleFile=true
dotnet publish --self-contained -r linux-x64 -c release -o ..\output\linux_x64 -p:PublishSingleFile=true
dotnet publish --self-contained -r osx-x64 -c release -o ..\output\darwin_x64 -p:PublishSingleFile=true
Set-Location ..

Set-Location output
Set-Location win32_x86
Compress-Archive * -DestinationPath ..\win32_x86.zip 
Set-Location ..
Set-Location win32_x64
Compress-Archive * -DestinationPath ..\win32_x64.zip
Set-Location ..
Set-Location linux_x64
chmod a+x Server
Compress-Archive * -DestinationPath ..\linux_x64.zip
Set-Location ..
Set-Location darwin_x64
chmod a+x Server
Compress-Archive * -DestinationPath ..\darwin_x64.zip
Set-Location ..
Set-Location ..

rmdir /s /q output
cd Server
call dotnet publish --self-contained -r win-x86 -c release -o ..\output\win32_x86
call dotnet publish --self-contained -r win-x64 -c release -o ..\output\win32_x64
call dotnet publish --self-contained -r linux-x64 -c release -o ..\output\linux_x64
call dotnet publish --self-contained -r osx-x64 -c release -o ..\output\darwin_x64
cd ..

cd output
cd win32_x86
..\..\7z.exe a ..\win32_x86.zip *
cd ..
cd win32_x64
..\..\7z.exe a ..\win32_x64.zip *
cd ..
cd linux_x64
..\..\7z.exe a ..\linux_x64.zip *
cd ..
cd darwin_x64
..\..\7z.exe a ..\darwin_x64.zip *
cd ..
cd ..




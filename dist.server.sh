#!/bin/bash

rm -r output
cd Server
dotnet publish --self-contained -r win-x86 -c release -o ../output/win32_x86
dotnet publish --self-contained -r win-x64 -c release -o ../output/win32_x64
dotnet publish --self-contained -r linux-x64 -c release -o ../output/linux_x64
dotnet publish --self-contained -r osx-x64 -c release -o ../output/darwin_x64
cd ..

cd output
cd win32_x86
zip -r ../win32_x86.zip *
cd ..
cd win32_x64
zip -r ../win32_x64.zip *
cd ..
cd linux_x64
zip -r ../linux_x64.zip *
cd ..
cd darwin_x64
zip -r ../darwin_x64.zip *
cd ..
cd ..

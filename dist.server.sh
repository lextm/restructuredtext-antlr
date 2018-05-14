#!/bin/bash

rm -r output
mkdir Server
cd Server
dotnet publish --self-contained -r win-x86 -c release -o ../output/win32_x86
dotnet publish --self-contained -r win-x64 -c release -o ../output/win32_x64
dotnet publish --self-contained -r linux-x64 -c release -o ../output/linux_x64
dotnet publish --self-contained -r osx-x64 -c release -o ../output/darwin_x64
cd ..

mkdir output
cd output
mkdir win32_x86
cd win32_x86
zip -r ../win32_x86.zip *
cd ..
mkdir win32_x64
cd win32_x64
zip -r ../win32_x64.zip *
cd ..
mkdir linux_x64
cd linux_x64
zip -r ../linux_x64.zip *
cd ..
mkdir darwin_x64
cd darwin_x64
zip -r ../darwin_x64.zip *
cd ..
cd ..

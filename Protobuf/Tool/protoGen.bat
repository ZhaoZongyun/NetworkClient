@echo off
SET PROTODIR=..\ProtoFiles\
SET CLIENT_CSHARPDIR=..\ProtoGen\
SET SERVER_CSHARPDIR=E:\CSharpProject\TestUDPServer\ProtoGen

for %%i in (%PROTODIR%*.proto) do (
    protoc-3.12.0-win64\bin\protoc.exe --proto_path=%PROTODIR% --csharp_out=%CLIENT_CSHARPDIR% %%i
)

for %%i in (%PROTODIR%*.proto) do (
    protoc-3.12.0-win64\bin\protoc.exe --proto_path=%PROTODIR% --csharp_out=%SERVER_CSHARPDIR% %%i
)

pause
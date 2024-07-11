@echo off
SET PROTODIR=..\ProtoFiles\
SET CLIENT_CSHARPDIR=..\ProtoGen\
@REM SET SERVER_CSHARPDIR=E:\CSharpProject\TestUDPServer\ProtoGen

for %%i in (%PROTODIR%*.proto) do (
    protoc-3.12.0-win64\bin\protoc.exe --proto_path=%PROTODIR% --csharp_out=%CLIENT_CSHARPDIR% %%i
)

@REM for %%i in (%PROTODIR%*.proto) do (
@REM     protoc-3.12.0-win64\bin\protoc.exe --proto_path=%PROTODIR% --csharp_out=%SERVER_CSHARPDIR% %%i
@REM )

pause
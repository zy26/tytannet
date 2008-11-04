@echo off

if {%1} == {} goto no_param
if {%2} == {} goto no_param
if {"%VS80COMNTOOLS%"} == {""} goto no_vs8
:vs_validate
if {"%VS80COMNTOOLS%"} == {""} goto no_vs9

REM #####################################################
echo Configuring environment for Visual Studio 2005...
call "%VS80COMNTOOLS%..\..\VC\vcvarsall.bat" x86
goto set_vars

REM #####################################################
echo Configuring environment for Visual Studio 2008...
call "%VS90COMNTOOLS%..\..\VC\vcvarsall.bat" x86
goto set_vars

REM #####################################################
:set_vars

set xpath=%1
set xnamespace=%2
set xkey=Key.snk
set xversion=1.1.0.0

pushd "%xpath%"
echo Creation of Interop assembly for ADO.NET ...
tlbimp /nologo /silent "C:\Program Files\Common Files\System\ADO\msado15.dll" /out:Pretorianie.AdoHelper.dll /keyfile:%xkey% /namespace:%xnamespace% /asmversion:%xversion%

echo Creation of Interop assembly for OLE DB ...
tlbimp /nologo /silent "C:\Program Files\Common Files\System\Ole DB\oledb32.dll" /out:Pretorianie.OleDbHelper.dll /keyfile:%xkey% /namespace:%xnamespace% /asmversion:%xversion%

popd

goto end

:no_vs8
echo No Visual Studio 2005 installed. Checking for never version.
goto vs_validate

:no_vs9
echo No Visual Studio 2008 installed on your machine!
goto end


:no_param
echo No valid params
echo Usage:
echo   CreateDatabaseInterop [ProjectPath] [Namespace]


:end

echo *** FINISHED ***

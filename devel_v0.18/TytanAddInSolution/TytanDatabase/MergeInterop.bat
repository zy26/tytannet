@echo off

if {%1} == {} goto no_param
if {%2} == {} goto no_param
if {%3} == {} goto no_param
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
set xtoolpath=%xpath:~1,-1%..\..\..\tools\ILMerge
set xoutput=%2
set xoutput=%xoutput:~1,-1%
set xtargetfile=%3
set xtargetfile=%xtargetfile:~1,-1%
set xoutputfile="%xoutput%%xtargetfile%.dll"
REM set xmergefile="bin\%xtargetfile%.dll"
set xkey=Key.snk

pushd "%xpath%"
echo Merging Database Interop Assemblies ...
%xtoolpath%\ilmerge.exe /ndebug /keyfile:%xkey% /target:library /copyattrs /out:%xoutputfile% %xoutputfile% Pretorianie.AdoHelper.dll Pretorianie.OleDbHelper.dll

REM echo Copying the result to the output directory (%xtargetfile%)...
REM copy  /V /Y /B %xmergefile% %xoutputfile%

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
echo   MergeInterop [ProjectPath] [OutputPath] [TargetName]


:end

echo *** FINISHED ***

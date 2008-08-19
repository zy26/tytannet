@echo off

REM echo Checking arguments... p1=%1, p2=%3

if {%1} == {} goto no_param
if {%2} == {} goto no_param
if {%3} == {} goto no_param
if {%4} == {} goto no_param
if {"%VS80COMNTOOLS%"} == {""} goto no_vs

echo Configuring environment...
call "%VS80COMNTOOLS%..\..\VC\vcvarsall.bat" x86

set xpath=%1
set xoutput=%2
set xoutput=%xoutput:~1,-1%
set xname=%3
set xresource=%4
set xculture=en-US

echo *** START ***
echo ProjectPath = %xpath%
echo OutputPath = %xoutput%
echo ProjectName = %xname%
echo Resource = %xresource%.resx

echo *** EXECUTION ***
echo Changing path...
pushd "%xpath%"

mkdir "%xoutput%" > NUL: 2> NUL:
mkdir "%xoutput%%xculture%" > NUL: 2>NUL:

echo Compiling resource...
( resgen.exe %xresource%.resx ) && (
al.exe /embed:%xresource%.resources /culture:%xculture% /out:"%xoutput%%xculture%\%xname%.resources.dll" ) && (
del %xresource%.resources )

echo Restoring path...
popd
goto end


:no_vs
echo No VisualStudio installed
goto end


:no_param
echo No valid params
echo Usage:
echo   EmbedResources [ProjectPath] [OutputPath] [ProjectName] [ResourceFile]


:end

echo *** FINISHED ***

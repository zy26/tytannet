@echo off

if {%1} == {} goto no_param
if {%2} == {} goto no_param
if {%3} == {} goto no_param
if {%4} == {} goto no_param
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
set xoutput=%2
set xoutput=%xoutput:~1,-1%
set xname=%3
set xresource=%4
set xculture=%5
set xversion=0.18.0.0
set xsignkey=key.snk

REM #####################################################
REM # generate assembly:                                #
REM #####################################################
echo *** START ***
REM echo ProjectPath = %xpath%
REM echo OutputPath = %xoutput%
REM echo ProjectName = %xname%
REM echo Resource = %xresource%.resx
echo Version = %xversion% (%xculture%)
echo SignKeyFile = %xsignkey%

echo *** EXECUTION ***
REM echo Changing path...
pushd "%xpath%"

mkdir "%xoutput%" > NUL: 2> NUL:
mkdir "%xoutput%%xculture%" > NUL: 2>NUL:

echo Compiling resource...
( resgen.exe %xresource%.resx ) && (
al.exe /embed:%xresource%.resources /culture:%xculture% /out:"%xoutput%%xculture%\%xname%.resources.dll" /v:%xversion% /keyf:%xsignkey% ) && (
del %xresource%.resources )

REM echo Restoring path...
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
echo   EmbedResources [ProjectPath] [OutputPath] [ProjectName] [ResourceFile]


:end

echo *** FINISHED ***

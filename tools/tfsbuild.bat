@ECHO ON

IF EXIST "e:\build_env2019.bat" (
call e:\build_env2019.bat
)

IF EXIST "%Build_BinariesDirectory%" (
    ECHO "Clean up Build_BinariesDirectory"
    DEL /Q/F/S %Build_BinariesDirectory%\*
)

echo "WE ARE GOING TO SOURCE DIRECTORY %BUILD_SOURCESDIRECTORY%"
IF EXIST "%BUILD_SOURCESDIRECTORY%" (
    cd %BUILD_SOURCESDIRECTORY%
)

msbuild PortalConsole.sln /t:restore /p:RestorePackagesConfig=true /p:GenerateProjectSpecificOutputFolder=true /p:OutDirWasSpecified=true
msbuild PortalConsole.sln /t:build

IF EXIST "%Build_BinariesDirectory%" (
    msbuild PortalConsole\PortalConsole.csproj /target:publish /p:PublishDir=%Build_BinariesDirectory%\
)

echo "Build completed. Message from build script"

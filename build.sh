#!/bin/bash
NOW=$(date +'%Y.%m.%d.%H%M%S')

echo $NOW

dotnet publish -c Release

dotnet pack Mechtanium/Mechtanium.csproj -p:NuspecFile=Mechtanium.nuspec -c Release -p:PackageVersion=$NOW --output bin/

dotnet nuget add source https://synchro.octopus.app/nuget/packages --name Modlium --username pavansinghx --password API-SILYYPYUR5WAZXXTDGBSUJDXZEU --store-password-in-clear-text

cd bin

dotnet nuget push *.nupkg -s https://synchro.octopus.app/nuget/packages -k API-SILYYPYUR5WAZXXTDGBSUJDXZEU
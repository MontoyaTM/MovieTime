#!/usr/bin/env bash
set -e

## install latest .NET 8.0 release
pushd /tmp
wget https://builds.dotnet.microsoft.com/dotnet/scripts/v1/dotnet-install.sh

chmod u+x /tmp/dotnet-install.sh
/tmp/dotnet-install.sh --channel 8.0
popd

## publish project to known location for subsequent deployment by Netlify
dotnet publish MovieTime.csproj -c Release -o release
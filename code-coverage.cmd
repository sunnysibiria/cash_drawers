@echo off
 
dotnet build
 
rem Instrument assemblies inside 'test' folder to detect hits for source files inside 'src' folder
dotnet minicover instrument --assemblies test/**/bin/Debug/**/*.dll --sources src/**/*.cs
 
rem Reset hits count in case minicover was run for this project
dotnet minicover reset
 
for /R test %%p in (*.csproj) do dotnet test --no-build %%p
 
rem Uninstrument assemblies, it's important if you're going to publish or deploy build outputs
dotnet minicover uninstrument
 
dotnet minicover report
 
mkdir coverage
 
dotnet minicover cloverreport --output coverage/clover.xml
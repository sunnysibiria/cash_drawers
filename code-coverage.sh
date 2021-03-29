cd src/CashManagment.Api

# Instrument assemblies inside 'test' folder to detect hits for source files inside 'src' folder
dotnet minicover instrument --workdir ../../ --assemblies test/**/bin/Debug/**/*.dll --sources src/**/*.cs 

# Reset hits count in case minicover was run for this project
dotnet minicover reset

cd ../../

for project in test/**/*.csproj; do dotnet test --no-build $project; done

cd src/CashManagment.Api

# Uninstrument assemblies, it's important if you're going to publish or deploy build outputs
dotnet minicover uninstrument --workdir ../../

dotnet minicover report --workdir ../../

mkdir ../../coverage

dotnet minicover cloverreport --workdir ../../ --output coverage/clover.xml

cd ../../
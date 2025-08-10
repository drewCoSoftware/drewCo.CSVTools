function Item-Exists($path) {

    $x = Get-Item -Path $path -ErrorAction SilentlyContinue
    return $x.Length -gt 0
}


# Get a clean copy of the build dir.
if (Item-Exists("nuget-build")) {
    Remove-Item "nuget-build" -Force -Recurse

    New-Item "nuget-build" -ItemType "Directory"
}
New-Item "nuget-build\lib\net8.0" -ItemType "Directory"


# Build our assemblies:
dotnet build drewCo.CSVTools.sln -p:Configuration=Release


#Copy the stuff we care about to the lib folder....

# NETCORE 8.0
Copy-Item -Path ".\drewCo.CSVTools\bin\Release\net8.0\drewCo.CSVTools.dll" "nuget-build\lib\net8.0\drewCo.CSVTools.dll"
Copy-Item -Path ".\drewCo.CSVTools\bin\Release\net8.0\drewCo.CSVTools.pdb" "nuget-build\lib\net8.0\drewCo.CSVTools.pdb"
Copy-Item -Path ".\drewCo.CSVTools\bin\Release\net8.0\drewCo.CSVTools.xml" "nuget-build\lib\net8.0\drewCo.CSVTools.xml"
Copy-Item -Path ".\drewCo.CSVTools\bin\Release\net8.0\drewCo.CSVTools.deps.json" "nuget-build\lib\net8.0\drewCo.CSVTools.deps.json"


# Copy everything to the build dir....
# Copy-Item -Path ".\lib" ".\nuget-build\lib" -Recurse
Copy-Item ".\drewCo.CSVTools.nuspec" ".\nuget-build\drewCo.CSVTools.nuspec"

# Pack it all up...
nuget pack ".\nuget-build\drewCo.CSVTools.nuspec"
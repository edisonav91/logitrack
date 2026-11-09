$ErrorActionPreference = 'Stop'

dotnet restore LogiTrack.sln --ignore-failed-sources
if ($LASTEXITCODE -ne 0) { throw 'Restore failed.' }

dotnet build LogiTrack.sln --no-restore
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }

$testProjects = @(
    'tests/LogiTrack.Domain.Tests/LogiTrack.Domain.Tests.csproj',
    'tests/LogiTrack.Application.Tests/LogiTrack.Application.Tests.csproj',
    'tests/LogiTrack.IntegrationTests/LogiTrack.IntegrationTests.csproj',
    'tests/LogiTrack.ArchitectureTests/LogiTrack.ArchitectureTests.csproj'
)

foreach ($project in $testProjects) {
    dotnet test $project --no-build --no-restore -m:1
    if ($LASTEXITCODE -ne 0) { throw "Tests failed for $project." }
}

dotnet format LogiTrack.sln --verify-no-changes --no-restore
if ($LASTEXITCODE -ne 0) { throw 'Format verification failed.' }

Write-Host 'All LogiTrack verification steps passed.'


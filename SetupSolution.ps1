# Define the microservice name
$microserviceName = "SquawkService"

# Define the project names and their respective paths within the microservice
$projectPaths = @{
    "ParrotInc.$microserviceName.Domain"       = "$microserviceName\Domain"
    "ParrotInc.$microserviceName.Application"   = "$microserviceName\Application"
    "ParrotInc.$microserviceName.Infrastructure" = "$microserviceName\Infrastructure"
    "ParrotInc.$microserviceName.API"           = "$microserviceName\API"
    "ParrotInc.$microserviceName.Tests"         = "$microserviceName\Tests"  # xUnit test project
}

# Create the solution in the current directory
$solutionPath = Join-Path -Path (Get-Location) -ChildPath "$microserviceName.sln"
dotnet new sln -n "$microserviceName" -o (Get-Location)

# Create each project and add to the solution
foreach ($project in $projectPaths.Keys) {
    # Create project folder
    $projectFolder = Join-Path -Path (Get-Location) -ChildPath $projectPaths[$project]
    New-Item -ItemType Directory -Path $projectFolder -Force

    # Create a class library project for each layer
    if ($project -eq "ParrotInc.$microserviceName.API") {
        # Create Web API project
        dotnet new webapi -n $project -o $projectFolder --no-https
    } elseif ($project -eq "ParrotInc.$microserviceName.Tests") {
        # Create xUnit test project
        dotnet new xunit -n $project -o $projectFolder
    } else {
        # Create class library project for other layers
        dotnet new classlib -n $project -o $projectFolder
    }

    # Add the project to the solution
    dotnet sln $solutionPath add "$($projectFolder)\$project.csproj"
}

# Add project references after all projects are created
dotnet add "$($projectPaths['ParrotInc.SquawkService.Application'])\ParrotInc.SquawkService.Application.csproj" reference "$($projectPaths['ParrotInc.SquawkService.Domain'])\ParrotInc.SquawkService.Domain.csproj"
dotnet add "$($projectPaths['ParrotInc.SquawkService.Infrastructure'])\ParrotInc.SquawkService.Infrastructure.csproj" reference "$($projectPaths['ParrotInc.SquawkService.Domain'])\ParrotInc.SquawkService.Domain.csproj"
dotnet add "$($projectPaths['ParrotInc.SquawkService.Infrastructure'])\ParrotInc.SquawkService.Infrastructure.csproj" reference "$($projectPaths['ParrotInc.SquawkService.Application'])\ParrotInc.SquawkService.Application.csproj"
dotnet add "$($projectPaths['ParrotInc.SquawkService.API'])\ParrotInc.SquawkService.API.csproj" reference "$($projectPaths['ParrotInc.SquawkService.Application'])\ParrotInc.SquawkService.Application.csproj"
dotnet add "$($projectPaths['ParrotInc.SquawkService.Tests'])\ParrotInc.SquawkService.Tests.csproj" reference "$($projectPaths['ParrotInc.SquawkService.Application'])\ParrotInc.SquawkService.Application.csproj"

# Output success message
Write-Host "Project structure for ParrotInc.SquawkService created successfully!"

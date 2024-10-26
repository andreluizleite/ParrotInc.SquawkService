# Set the base folder to the current directory
$baseFolder = (Get-Location).Path

# Create classes for Domain layer
$domainClasses = @(
    "Entities/Squawk.cs",
    "Entities/User.cs",
    "ValueObjects/SquawkContent.cs",
    "ValueObjects/SquawkMetadata.cs",
    "Interfaces/ISquawkRepository.cs",
    "DomainServices/SquawkDomainService.cs",
    "Specifications/SquawkSpecification.cs"  
)

# Create Domain classes
foreach ($class in $domainClasses) {
    New-Item -Path "$baseFolder\Domain\$class" -ItemType File -Force
}

# Create classes for Application layer
$appClasses = @(
    "Commands/SquawkCommand.cs",
    "Queries/SquawkQuery.cs",
    "CommandHandlers/SquawkCommandHandler.cs",
    "QueryHandlers/SquawkQueryHandler.cs",
    "DTOs/SquawkDTO.cs"  # Data Transfer Object example
)

# Create Application classes
foreach ($class in $appClasses) {
    New-Item -Path "$baseFolder\Application\$class" -ItemType File -Force
}

# Create classes for Infrastructure layer
$infrastructureClasses = @(
    "Repositories/SquawkRepository.cs",
    "DatabaseContext.cs",
    "LoggingService.cs",
    "Configuration/AppSettings.cs"
)

# Create Infrastructure classes
foreach ($class in $infrastructureClasses) {
    New-Item -Path "$baseFolder\Infrastructure\$class" -ItemType File -Force
}

# Create classes for API layer
$apiClasses = @(
    "Controllers/SquawkController.cs",
    "RequestModels/SquawkRequestModel.cs",
    "ResponseModels/SquawkResponseModel.cs",
    "MinimalAPIs/SquawkEndpoints.cs"  # Minimal API endpoint example
)

# Create API classes
foreach ($class in $apiClasses) {
    New-Item -Path "$baseFolder\API\$class" -ItemType File -Force
}

# Output completion message
Write-Host "Class files created successfully in $baseFolder."

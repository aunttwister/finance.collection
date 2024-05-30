param (
    [string]$configFilePath = ".\config.json"
)

# Function to display status messages with timestamps
function Show-Status {
    param (
        [string]$message
    )
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "[INFO] [$timestamp] $message"
    Write-Host $logMessage
    Add-Content -Path $logFile -Value $logMessage
}

# Function to display error messages with timestamps
function Show-Error {
    param (
        [string]$message
    )
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "[ERROR] [$timestamp] $message"
    Write-Host $logMessage -ForegroundColor Red
    Add-Content -Path $logFile -Value $logMessage
    exit 1
}

# Function to extract image names from a given string
function Extract-ImageNames {
    param (
        [string]$content
    )

    # Initialize an array to hold extracted image names
    $imageNames = @()

    # Define the regex pattern to match ![[image.png]]
    $pattern = '!\[\[(.*?)\.png\]\]'

    # Use regex to find all matches
    $matches = [regex]::Matches($content, $pattern)

    # Iterate through the matches and extract the image names
    foreach ($match in $matches) {
        $imageNames += $match.Groups[1].Value + ".png"
    }

    # Return the array of extracted image names
    return $imageNames
}

# Function to clear a directory
function Clear-Directory {
    param (
        [string]$directory
    )
    if (Test-Path $directory) {
        Get-ChildItem -Path $directory -Recurse | Remove-Item -Recurse -Force
        Show-Status "Cleared directory: $directory"
    }
}

# Log file
$logFile = "$(Split-Path -Path $MyInvocation.MyCommand.Path -Parent)\build-log.txt"

# Read configuration file
if (-Not (Test-Path $configFilePath)) {
    Show-Error "Configuration file '$configFilePath' does not exist."
}

$config = Get-Content -Path $configFilePath | ConvertFrom-Json
$sourceDir = $config.sourceDir
$targetDir = $config.targetDir
$mediaDir = $config.mediaDir

# Debug message to confirm script execution
Show-Status "Starting the file copy script with markdown image link replacement..."

# Check if source and target directories exist
if (-Not (Test-Path $sourceDir)) {
    Show-Error "Source directory '$sourceDir' does not exist."
}
if (-Not (Test-Path $targetDir)) {
    Show-Error "Target directory '$targetDir' does not exist."
}
if (-Not (Test-Path $mediaDir)) {
    Show-Error "Media target directory '$mediaDir' does not exist."
}

# Clear target directories
Clear-Directory $targetDir
Clear-Directory $mediaDir

# Process and copy Markdown and PNG files from source to target directory
try {
    Show-Status "Processing files from '$sourceDir'..."

    # Get all Markdown and PNG files in the source directory
    $files = Get-ChildItem -Path $sourceDir -Include *.md, *.png -Recurse

    foreach ($file in $files) {
        Show-Status "Processing file '$($file.FullName)'..."

        # Define the target directory structure
        $relativePath = $file.FullName.Substring($sourceDir.Length).TrimStart('\')
        $targetFilePath = Join-Path -Path $targetDir -ChildPath $relativePath

        # Ensure the target directory exists
        $targetDirectory = Split-Path -Path $targetFilePath -Parent
        if (-Not (Test-Path $targetDirectory)) {
            New-Item -Path $targetDirectory -ItemType Directory | Out-Null
        }

        if ($file.Extension -eq ".md") {
            # Read the content of the Markdown file
            $content = Get-Content -Path $file.FullName

            # Log before replacement
            Show-Status "Reading content from '$($file.FullName)'"

            # Replace the image links
            $imageNameMatches = Extract-ImageNames $content

            foreach ($match in $imageNameMatches) {
                $replaceExpression = "![[$match]]"
                $replaceTarget = "![${match}](https://localhost:5001/media/$match)"
                Show-Status "Replacing '$replaceExpression' with '$replaceTarget'"
                $content = $content -replace [regex]::Escape($replaceExpression), $replaceTarget
            }

            # Save the modified content to the target directory
            Set-Content -Path $targetFilePath -Value $content
            Show-Status "File '$($file.Name)' processed and saved to '$targetFilePath'."
        } elseif ($file.Extension -eq ".png") {
            # Copy the PNG file to the media directory
            Copy-Item -Path $file.FullName -Destination $mediaDir -Force
            Show-Status "File '$($file.Name)' copied to '$mediaDir'."
        }
    }

    Show-Status "All files processed successfully."
} catch {
    Show-Error "An error occurred while processing files: $_"
}

# Final status message
Show-Status "File copy and processing script completed successfully."
exit 0

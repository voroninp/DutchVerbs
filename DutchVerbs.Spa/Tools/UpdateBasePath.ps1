param(
    [Parameter(Mandatory=$true)]
    [string]$filePath,

    [Parameter(Mandatory=$true)]
    [string]$sitePath
)

if (-Not (Test-Path -Path $filePath)) {
    Write-Error "The file '$filePath' does not exist."
    exit 1
}

if ([System.String]::IsNullOrWhitespace($sitePath)) {
    Write-Error "Site path is empty."
    exit 1
}

# Read the content of the file
$content = Get-Content $filePath

# Check if the base tag exists
if ($content -match '<base.*href=".*".*>') {
    # Update the href attribute of the existing base tag
    $updatedContent = $content -replace '<base.*href=".*".*>', "<base href=`"$sitePath`">"
} else {
    # If the base tag doesn't exist, insert it in the head section
    $updatedContent = $content -replace '<head>', "<head>`r`n    <base href=`"$sitePath`">"
}

# Write the updated content back to the file
$updatedContent | Set-Content $filePath

Write-Output "Updated the href attribute of the base element in $filePath"

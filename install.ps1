# Define the repository owner and name
$owner = "temannin"
$repo = "ControllerOnConnect"

# Get the latest release from GitHub API
$apiUrl = "https://api.github.com/repos/$owner/$repo/releases/latest"
$release = Invoke-RestMethod -Uri $apiUrl -UseBasicParsing

echo $release

# Find the .exe asset in the latest release
$exeAsset = $release.assets | Where-Object { $_.name -like "*.exe" }

# If an .exe file was found, download it
if ($exeAsset) {
    $downloadUrl = $exeAsset.browser_download_url
    $outputFile = "$env:TEMP\$($exeAsset.name)"  # Download to temporary folder

    # Download the .exe file
    Invoke-WebRequest -Uri $downloadUrl -OutFile $outputFile

    Write-Host "Downloaded: $outputFile"
} else {
    Write-Host "No .exe file found in the latest release."
}

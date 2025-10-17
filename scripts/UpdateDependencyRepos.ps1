
# Clones or updates the depencencies repositories for IGLibGraphics3D.
Write-Host "`n`nCloning / updating basic dependency repositories of IGLibGraphics3D ...`n"

# Get the script directory such that relative paths can be resolved:
$scriptPath = $MyInvocation.MyCommand.Path
$scriptDir = Split-Path $scriptPath -Parent
$scriptFilename = [System.IO.Path]::GetFileName($scriptPath)

Write-Host "Script directory: $scriptDir"

Write-Host "`nUpdating IGLibCore:"
& $(Join-Path $scriptDir "UpdateRepo_IGLibCore.ps1")

Write-Host "  ... updating IGLibGraphics3D dependencies complete.`n`n"


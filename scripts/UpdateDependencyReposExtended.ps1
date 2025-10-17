
# Clones or updates the depencencies repositories for IGLibGraphics3D.
Write-Host "`n`nCloning / updating EXTENDED dependency repositories of IGLibGraphics3D ..."

# Get the script directory such that relative paths can be resolved:
$scriptPath = $MyInvocation.MyCommand.Path
$scriptDir = Split-Path $scriptPath -Parent
$scriptFilename = [System.IO.Path]::GetFileName($scriptPath)

Write-Host "Script directory: $scriptDir"

# Write-Host "`nUpdating basic dependencies of IGLibGraphics3D:`n" # this output already in script.
& $(join-path $scriptDir "UpdateDependencyRepos.ps1")

Write-Host "`nUpdating extended dependencies of IGLibGraphics3D:`n"

Write-Host "`nUpdating IGLibCore:"
& $(Join-Path $scriptDir "UpdateRepo_IGLibScripts.ps1")


Write-Host "  ... updating IGLibGraphics3D EXTENDED dependencies complete.`n`n"


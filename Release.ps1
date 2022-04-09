param(
[string] $version
)
$releaseDate = Get-Date -Format "yyyy-MM-dd"
(Get-Content changelog.md).Replace("## [Unreleased]", "## [$version] - $releaseDate") | Set-Content changelog.md
git add changelog.md
git commit -m "Updated changelog with release $version"
git tag $version
dotnet publish -c Release -p:Version=$version -o release
Compress-Archive -Force -Path release/* -DestinationPath Rstolsmark.WakeOnLanServer.zip
Remove-Item release -Recurse -Force

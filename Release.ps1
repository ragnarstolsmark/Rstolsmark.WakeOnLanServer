param(
    [Parameter(Mandatory=$true)]
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
docker login -u rstolsmark
docker build --build-arg VERSION=$version -t rstolsmark/wakeonlanserver:$version .
docker push rstolsmark/wakeonlanserver:$version
docker logout
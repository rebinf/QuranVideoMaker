# cd to current script directory
$scriptPath = $MyInvocation.MyCommand.Path;
$scriptDirectory = Split-Path -Path $scriptPath -Parent;
Set-Location -Path $scriptDirectory;

$deletePublishDir = $true;
$downloadFfmpeg = $true;
$moveDependencies = $false;
$zipPublishDir = $true;

# update version
$version = "1.0.0";

# get version from github run number
if ($env:GITHUB_RUN_NUMBER)
{
    $version = "0.0.$env:GITHUB_RUN_NUMBER";

    # update version in VERSION.txt file
    Set-Content -Path ./QuranVideoMaker/Resources/VERSION.txt -Value $version;
}

Write-Output "Version: $version";

# cd to src directory
Set-Location -Path src;

if ($deletePublishDir)
{
    # delete publish directory
    Write-Output "Deleting publish directory...";
    Remove-Item -Path $publishDir -Recurse -Force;
}

# restore nuget packages
Write-Output "Restoring nuget packages...";
dotnet restore;

# publish solution
Write-Output "Publishing solution...";
dotnet publish -c release -r win-x64 --self-contained;

# publish dir
$releaseDir = "./QuranVideoMaker/bin/Release";
$publishDir = "${releaseDir}/net9.0-windows/win-x64/publish";
Write-Output "Publish directory: $publishDir";

if ($downloadFfmpeg)
{
    # download ffmpeg
    Write-Output "Downloading ffmpeg...";
    $ffmpegZipPath = "${publishDir}/ffmpeg.zip";
    $ffmpegUnzipPath = "${publishDir}/ffmpeg_all";
    $ffmpegDir = "${publishDir}/ffmpeg";
    Invoke-WebRequest -Uri https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl-shared.zip -OutFile $ffmpegZipPath;
    
    # unzip ffmpeg
    Write-Output "Unzipping ffmpeg...";
    Expand-Archive -Path $ffmpegZipPath -DestinationPath $ffmpegUnzipPath -Force;
    
    # copy ffmpeg files
    Write-Output "Copying ffmpeg files...";
    
    # if ffmpeg directory exists, delete it
    if (Test-Path -Path $ffmpegDir)
    {
        Remove-Item -Path $ffmpegDir -Recurse -Force;
    }
    
    Copy-Item -Path "${ffmpegUnzipPath}/ffmpeg-master-latest-win64-gpl-shared/bin/" -Destination $ffmpegDir -Recurse -Force;
    
    # copy ffmpeg to publish directory
    Write-Output "Delete ffmpeg archive...";
    Remove-Item -Path $publishDir/ffmpeg.zip -Force;
    
    # delete ffmpeg_unzip directory
    Write-Output "Delete ffmpeg_unzip directory...";
    Remove-Item -Path $ffmpegUnzipPath -Recurse -Force;
}

# delete locale files
Write-Output "Deleting locale files...";
$localDirNames = ("cs", "de", "es", "fr", "it", "ja", "ko", "pl", "pt-BR", "ru", "tr", "zh-Hans", "zh-Hant");

foreach ($dirName in $localDirNames)
{
    if (Test-Path -Path "${publishDir}/${dirName}")
    {
        Remove-Item -Path "${publishDir}/${dirName}" -Recurse -Force;
    }
}

# copy QuranImageMaker.App* files to QuranVideoMaker publish directory
Write-Output "Copying QuranImageMaker.App* files to QuranVideoMaker publish directory...";
Copy-Item -Path ./QuranImageMaker.App/bin/Release/net9.0-windows/win-x64/publish/QuranImageMaker.App* -Destination $publishDir -Recurse -Force

if ($moveDependencies)
{
    # move dependencies to publish/lib directory
    Write-Output "Moving dependencies to publish/lib directory...";
    
    # if lib directory exists, delete it
    $libDir = "${publishDir}/lib";
    if (Test-Path -Path $libDir)
    {
        Remove-Item -Path $libDir -Recurse -Force;
    }
    
    # create lib directory
    New-Item -Path $libDir -ItemType Directory;
    
    # copy dependencies
    $dependencies = Get-ChildItem -Path "${publishDir}" -Include "*.dll", "*.pdb" -Recurse;
    foreach ($dependency in $dependencies)
    {
        # if file name starts with QuranVideoMaker, skip it
        if ($dependency.Name.StartsWith("QuranVideoMaker"))
        {
            continue;
        }
    
        # if file is hostfxr.dll, hostpolicy.dll, or icudt*.dll, skip it
        if ($dependency.Name -eq "hostfxr.dll" -or $dependency.Name -eq "hostpolicy.dll" -or $dependency.Name.StartsWith("icudt"))
        {
            continue;
        }
    
        $dependencyPath = $dependency.FullName;
        $dependencyName = $dependency.Name;
        $dependencyDestPath = "${libDir}/${dependencyName}";
        Move-Item -Path $dependencyPath -Destination $dependencyDestPath -Force;
    }
}

if ($zipPublishDir)
{
    # zip publish directory
    Write-Output "Zipping publish directory...";
    $zipFilePath = "${releaseDir}/QuranVideoMaker.zip";
    Compress-Archive -Path "${publishDir}/*" -DestinationPath $zipFilePath -Force;

    # set output zip file path as environment variable
    Write-Output "Set output zip file path as environment variable...";
    $env:QURAN_VIDEO_MAKER_ZIP_FILE_PATH = $zipFilePath;
}


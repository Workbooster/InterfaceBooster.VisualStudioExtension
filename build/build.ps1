properties { 
  $majorVersion = "1.0"
  $majorWithReleaseVersion = "1.0.1"
  $version = GetVersion $majorWithReleaseVersion
  $signAssemblies = $false
  $signKeyPath = "C:\key.snk"
  $treatWarningsAsErrors = $false
  
  $baseDir  = resolve-path ..
  $buildDir = "$baseDir\build"
  $sourceDir = "$baseDir\src"
  $toolsDir = "$baseDir\tools"
  $workingDir = "$baseDir\release"
  
  $extensionProjectName = "InterfaceBooster.VisualStudioExtension"
  $extensionProjectDir = "$sourceDir\$extensionProjectName"
  
  $builds = @(
      @{Name = "InterfaceBooster.VisualStudioExtension"; TestsName = ""; TestsFunction = ""; Constants=""; FinalDir="Net45"; NuGetDir = "net45"; Framework="net-4.5"; Sign=$false; Unsafe=$true}
  )
}

$framework = '4.0x86'

task default -depends Test

# Ensure a clean working directory
task Clean {
  Set-Location $baseDir
  
  if (Test-Path -path $workingDir)
  {
    Write-Output "Deleting Working Directory"
    
    del $workingDir -Recurse -Force
  }
  
  New-Item -Path $workingDir -ItemType Directory
}

# Build each solution, optionally signed
task Build -depends Clean { 
  Write-Host -ForegroundColor Green "Updating assembly version"
  
  $projectTplPath = "$extensionProjectDir\ProjectTemplates"
  
  # Create Provider Plugin Template Package
  
  $ppTplPath = "$sourceDir\InterfaceBooster.ProviderPluginTemplate\*"
  $ppTplZipPath = "$workingDir\InterfaceBooster.ProviderPluginTemplate.zip"
  
  ZipFiles $ppTplPath $ppTplZipPath
  
  # Copy the Provider Plugin Template to the Visual Studio Extension Project
  
  Copy-Item $ppTplZipPath -destination $projectTplPath -recurse
  
  # Remove the Provider Plugin Template ZIP File from the Release dir
  
  del $ppTplZipPath
  
  # Build the Visual Studio Extension Project
  
  foreach ($build in $builds)
  {
    $name = $build.Name
    if ($name -ne $null)
    {
      $finalDir = $build.FinalDir
      $sign = ($build.Sign -and $signAssemblies)
      $unsafe = $build.Unsafe

      Write-Host -ForegroundColor Green "Building " $name
      Write-Host -ForegroundColor Green "Signed " $sign

      Write-Host
      Write-Host "Building"
      exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release "/p:Platform=x86" /p:OutputPath=bin\Release\$finalDir\ /p:AssemblyOriginatorKeyFile=$signKeyPath "/p:SignAssembly=$sign" "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=11.0" "/p:AllowUnsafeBlocks=$unsafe" (GetConstants $build.Constants $sign) "$sourceDir\$name\$name.csproj" | Out-Default } "Error building $name"
    }
  }
}

# Builds and copies the assemblies to a release directory
task Package -depends Build {
  foreach ($build in $builds)
  {
    $name = $build.Name
    $finalDir = $build.FinalDir
    
    # Only copy Visual Studio Extension .vsix File
    
    robocopy "$sourceDir\$name\bin\Release\$finalDir" $workingDir *.vsix /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
  }
}

task Test -depends Deploy {
  foreach ($build in $builds)
  {
    if ($build.TestsFunction -ne $null)
    {
      & $build.TestsFunction $build
    }
  }
}

function ZipFiles($sourcedir, $zipfilename)
{
    [string]$pathToZipExe = "$toolsDir\7-Zip\7za.exe";
    [Array]$arguments = "a", "-tzip", "$zipfilename", "$sourcedir", "-r";
    & $pathToZipExe $arguments;
}

function GetConstants($constants, $includeSigned)
{
  $signed = switch($includeSigned) { $true { ";SIGNED" } default { "" } }

  return "/p:DefineConstants=`"CODE_ANALYSIS;TRACE;$constants$signed`""
}

function GetVersion($majorVersion)
{
    $now = [DateTime]::Now
    
    $year = $now.Year - 2000
    $month = $now.Month
    $totalMonthsSince2000 = ($year * 12) + $month
    $day = $now.Day
    $minor = "{0}{1:00}" -f $totalMonthsSince2000, $day
    
    $hour = $now.Hour
    $minute = $now.Minute
    $revision = "{0:00}{1:00}" -f $hour, $minute
    
    return $majorVersion + "." + $minor
}

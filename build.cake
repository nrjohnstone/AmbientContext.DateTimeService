#addin "Newtonsoft.Json&version=12.0.2"
#addin "Cake.Powershell&version=0.4.8"

#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"
#tool "nuget:?package=xunit.runner.console&version=2.4.1"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var packageVersion = "1.0.0";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var solutionDir = Directory("./");
var solutionFile = solutionDir + File("AmbientContext.DateTimeService.sln");
var projectDir = Directory("./src/AmbientContext.DateTimeService");

var artifactsDir = MakeAbsolute(Directory("artifacts"));
var packagesDir = artifactsDir.Combine(Directory("packages"));

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Clean")
.Does(() => {
	CleanDirectory(artifactsDir);
	DotNetCoreClean(solutionFile);
});


Task("Restore")
.IsDependentOn("Clean")
.Does(() =>
{
    DotNetCoreRestore(solutionFile);
});


Task("Pack")
.IsDependentOn("Build")
.Does(() =>  {
	var settings = new DotNetCorePackSettings
	{
		Configuration = configuration,
		NoBuild = true,
		NoRestore = true,
		IncludeSymbols = true,
		OutputDirectory = packagesDir,
		MSBuildSettings = new DotNetCoreMSBuildSettings()
								.WithProperty("PackageVersion", packageVersion)
								.WithProperty("Copyright", $"Copyright � Nathan Johnstone {DateTime.Now.Year}")
	};

	GetFiles("./src/*/*.csproj")
		.ToList()
		.ForEach(f => DotNetCorePack(f.FullPath, settings));
});

Task("Update-Version")
.IsDependentOn("Restore")
.Does(() => {
	try {
		var outputType = GitVersionOutput.Json;
    
		if (AppVeyor.IsRunningOnAppVeyor)
		{
			outputType = GitVersionOutput.BuildServer;
		}

		var gitVersion = GitVersion(new GitVersionSettings { 
			OutputType = outputType,
			NoFetch = true,
			Verbosity = GitVersionVerbosity.Debug
		});
		packageVersion = gitVersion.NuGetVersion;

		Information($"NuGetVersion: {packageVersion}");
	} catch(Exception ex) {
		Error(ex.Message);
	}
});

Task("Build")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Update-Version")
.Does(() => {
	var settings = new DotNetCoreBuildSettings
	{
		Configuration = configuration,
		NoIncremental = true,
		NoRestore = true,
		MSBuildSettings = new DotNetCoreMSBuildSettings()
								.SetVersion(packageVersion)
								.WithProperty("FileVersion", packageVersion)
								.WithProperty("InformationalVersion", packageVersion)
								.WithProperty("Copyright", $"Copyright © Nathan Johnstone {DateTime.Now.Year}")
	};
	DotNetCoreBuild(solutionFile, settings);
});


Task("Run-Unit-Tests")
.Does(() => {
    //var testAssemblies = GetFiles(".\\test\\AmbientContext.Tests\\bin\\" + configuration + "\\net451\\*\\AmbientContext.Tests.dll");
    //Console.WriteLine(testAssemblies.Count());
    //XUnit2(testAssemblies);
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack");
    
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

#addin "Newtonsoft.Json"
#addin "Cake.Powershell"
#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=xunit.runner.console"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var solutionDir = Directory("./");
var solutionFile = solutionDir + File("AmbientContext.DateTimeService.sln");
var projectDir = Directory("./src/AmbientContext.DateTimeService");
var buildDir = projectDir + Directory("bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});


Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});


Task("Pack")
    .Does(() => 
{
    EnsureDirectoryExists("./artifacts");
    string version = GitVersion().NuGetVersion;

    var binDir = Directory("./bin") ;
    var nugetPackageDir = Directory("./artifacts");

    var nugetFilePaths = GetFiles("./src/AmbientContext.DateTimeService/*.csproj");

    var nuGetPackSettings = new NuGetPackSettings
    {   
        Version = version,
        BasePath = binDir + Directory(configuration),
        OutputDirectory = nugetPackageDir,
        ArgumentCustomization = args => args.Append("-Prop Configuration=" + configuration)
    };

    NuGetPack(nugetFilePaths, nuGetPackSettings);
});


Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Update-Version")
    .Does(() =>
{
    DotNetCoreRestore();

    // Use MSBuild
    MSBuild(solutionFile, settings =>
    settings.SetConfiguration(configuration));
});


Task("Run-Unit-Tests")
    .Does(() =>
{
    //var testAssemblies = GetFiles(".\\test\\AmbientContext.Tests\\bin\\" + configuration + "\\net451\\*\\AmbientContext.Tests.dll");
    //Console.WriteLine(testAssemblies.Count());
    //XUnit2(testAssemblies);
});


Task("Update-Version")
    .Does(() => 
{
     var assemblyInfoFile = File(projectDir + File("Properties/AssemblyVersionInfo.cs"));

    if (!FileExists(assemblyInfoFile))
    {
        Information("Assembly version file does not exist : " + assemblyInfoFile.Path);
        CopyFile(projectDir + File("./AssemblyVersionInfo.template.cs"), assemblyInfoFile);
    }
    var outputType = GitVersionOutput.Json;
    
    if (AppVeyor.IsRunningOnAppVeyor)
    {
        outputType = GitVersionOutput.BuildServer;
    }

    GitVersion(new GitVersionSettings { 
        OutputType = outputType,
        UpdateAssemblyInfo = true,
		UpdateAssemblyInfoFilePath = assemblyInfoFile });

    string version = GitVersion().NuGetVersion;
	Console.WriteLine("New version string =" + version);
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

#tool "nuget:?package=xunit.runner.console"

var target = Argument( "target", "Default" );
var configuration = Argument( "configuration", "Release" );

var buildDir = Directory( "./src/GitWrite/bin" ) + Directory( configuration );

//===========================================================================
// Clean Task
//===========================================================================

Task( "Clean" )
   .Does( () =>
{
   CleanDirectory( buildDir );
});

//===========================================================================
// Restore Task
//===========================================================================

Task( "RestoreNuGetPackages" )
   .IsDependentOn( "Clean" )
   .Does( () =>
{
   NuGetRestore( "./src/GitWrite.sln" );
} );

//===========================================================================
// Build Task
//===========================================================================

Task( "Build" )
   .IsDependentOn( "RestoreNuGetPackages")
   .Does( () =>
{
  MSBuild( "./src/GitWrite.sln", settings => settings.SetConfiguration( configuration ) );
} );

//===========================================================================
// Unit Test Task
//===========================================================================

Task( "RunUnitTests" )
   .IsDependentOn( "Build" )
   .Does( () =>
{
    XUnit2( "./src/GitWrite.UnitTests/bin/" + Directory( configuration ) + "/*Tests*.dll" );
} );

//===========================================================================
// Integration Test Task
//===========================================================================

Task( "RunIntegrationTests" )
   .IsDependentOn( "RunUnitTests" )
   .Does( () =>
{
    XUnit2( "./src/GitWrite.IntegrationTests/bin/" + Directory( configuration ) + "/GitWrite.IntegrationTests.dll" );
} );

//===========================================================================
// Default Task
//===========================================================================

Task( "Default" )
   .IsDependentOn( "RunIntegrationTests" );

RunTarget( target );

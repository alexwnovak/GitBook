#tool "nuget:?package=xunit.runner.console"

var target = Argument( "target", "Default" );
var configuration = Argument( "configuration", "Release" );

var buildDir = Directory( "./src/GitWrite/GitWrite/bin" ) + Directory( configuration );

Task( "Clean" )
   .Does( () =>
{
   CleanDirectory( buildDir );
});

Task( "RestoreNuGetPackages" )
   .IsDependentOn( "Clean" )
   .Does( () =>
{
   NuGetRestore( "./src/GitWrite/GitWrite.sln" );
} );

Task( "Build" )
   .IsDependentOn( "RestoreNuGetPackages")
   .Does( () =>
{
  MSBuild( "./src/GitWrite/GitWrite.sln", settings => settings.SetConfiguration( configuration ) );
} );

Task( "RunUnitTests" )
   .IsDependentOn( "Build" )
   .Does( () =>
{
    XUnit2( "./src/GitWrite/GitWrite.UnitTests/bin/" + Directory( configuration ) + "/*Tests*.dll" );
} );

Task( "Default" )
   .IsDependentOn( "RunUnitTests" );

RunTarget( target );

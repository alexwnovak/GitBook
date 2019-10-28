using AutoFixture;

namespace GitWrite.Tests.Internal
{
   internal static class FixtureExtensions
   {
      public static IFixture RegisterFunction<TFunctionType>( this IFixture fixture, TFunctionType function )
      {
         fixture.Register<TFunctionType>( () => function );
         return fixture;
      }
   }
}

using System;
using System.Reflection;
using Xunit.Sdk;

namespace GitWrite.IntegrationTests.Infrastructure
{
   [AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true )]
   public class AutoDisposeAttribute : BeforeAfterTestAttribute
   {
      public override void After( MethodInfo methodUnderTest )
      {
         DisposalRegistry.Instance.Dispose();
      }
   }
}
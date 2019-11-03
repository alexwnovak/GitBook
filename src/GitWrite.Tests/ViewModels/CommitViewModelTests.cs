using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using GitWrite.Tests.Internal;
using GitWrite.ViewModels;
using Xunit;

namespace GitWrite.Tests.ViewModels
{
   public class CommitViewModelTests
   {
      [Theory, AutoData]
      public void TestThing( IFixture fixture )
      {
         fixture.RegisterFunction<GetCommitFilePathFunction>( () => "asd" );

         var sut = fixture.Create<CommitViewModel>();
      }
   }
}

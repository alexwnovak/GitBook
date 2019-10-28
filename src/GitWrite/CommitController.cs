//using System;
//using GalaSoft.MvvmLight.Ioc;

//namespace GitWrite
//{
//   public class CommitController : IAppController
//   {
//      private readonly IObjectComposer _objectComposer = new CommitObjectComposer( SimpleIoc.Default );

//      public CommitController( string filePath )
//      {
//         _objectComposer.Compose();
//         _objectComposer.Container.Register( () => filePath );
//      }

//      public Uri GetStartupUri()
//      {
//         return new Uri( @"Views\CommitWindow.xaml", UriKind.Relative );
//      }
//   }
//}

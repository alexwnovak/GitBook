//using System;
//using System.IO;
//using System.Windows;
//using GitModel;

//namespace GitWrite
//{
//   internal static class AppControllerFactory
//   {
//      public static IAppController GetController( StartupEventArgs e )
//      {
//         IAppController appController = null;
//         string fileName = Path.GetFileName( e.Args[0] );

//         switch ( fileName )
//         {
//            case GitFileNames.CommitFileName:
//               appController = new CommitController( e.Args[0] );
//               break;
//            default:
//               MessageBox.Show( $"Unsupported workflow for file: {e.Args[0]}" );
//               Environment.Exit( 1 );
//               break;
//         }

//         return appController;
//      }
//   }
//}

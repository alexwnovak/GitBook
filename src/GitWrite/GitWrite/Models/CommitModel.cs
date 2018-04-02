using GalaSoft.MvvmLight;

namespace GitWrite.Models
{
   public class CommitModel : ObservableObject
   {
      private string _subject;
      public string Subject
      {
         get => _subject;
         set => Set( () => Subject, ref _subject, value );
      }

      private string[] _body;
      public string[] Body
      {
         get => _body;
         set => Set( () => Body, ref _body, value );
      }
   }
}

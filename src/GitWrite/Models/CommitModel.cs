using Caliburn.Micro;

namespace GitWrite.Models
{
   public class CommitModel : PropertyChangedBase
   {
      private string _subject;
      public string Subject
      {
         get => _subject;
         set
         {
            _subject = value;
            NotifyOfPropertyChange( nameof( Subject ) );
         }
      }

      private string _body;
      public string Body
      {
         get => _body;
         set
         {
            _body = value;
            NotifyOfPropertyChange( nameof( Body ) );
         }
      }
   }
}

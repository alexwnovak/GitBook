using System.Media;
using GitWrite.Properties;

namespace GitWrite.Services
{
   public class SoundService : ISoundService
   {
      public void PlayPopSound()
      {
         var soundPlayer = new SoundPlayer
         {
            Stream = Resources.PopSound
         };

         soundPlayer.Play();
      }
   }
}

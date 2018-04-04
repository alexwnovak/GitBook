using GalaSoft.MvvmLight.Ioc;

namespace GitWrite
{
   public interface IObjectComposer
   {
      ISimpleIoc Container { get; }
      void Compose();
   }
}

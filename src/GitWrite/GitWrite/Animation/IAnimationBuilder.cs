namespace GitWrite.Animation
{
   public interface IAnimationBuilder<in T>
   {
      IAnimationBuilder<T> From( T value );
      IAnimationBuilder<T> To( T value );
      IAnimationBuilder<T> For( double milliseconds );
      void Begin();
   }
}

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GitWrite.Animation
{
   public abstract class AnimationBuilderBase<T> : IAnimationBuilder<T>
   {
      protected FrameworkElement FrameworkElement
      {
         get;
      }

      protected DependencyProperty DependencyProperty
      {
         get;
      }

      protected T FromValue
      {
         get;
         private set;
      }

      protected T ToValue
      {
         get;
         private set;
      }

      protected double ForValue
      {
         get;
         private set;
      }

      protected AnimationBuilderBase( FrameworkElement frameworkElement, DependencyProperty dependencyProperty )
      {
         FrameworkElement = frameworkElement;
         DependencyProperty = dependencyProperty;
      }

      public IAnimationBuilder<T> From( T value )
      {
         FromValue = value;
         return this;
      }

      public IAnimationBuilder<T> To( T value )
      {
         ToValue = value;
         return this;
      }

      public IAnimationBuilder<T> For( double milliseconds )
      {
         ForValue = milliseconds;
         return this;
      }

      public abstract void Begin();
   }

   public class DoubleAnimationBuilder : AnimationBuilderBase<double>
   {
      public DoubleAnimationBuilder( FrameworkElement frameworkElement, DependencyProperty dependencyProperty )
         : base( frameworkElement, dependencyProperty )
      {
      }

      public override void Begin()
      {
         var animation = new DoubleAnimation
         {
            From = FromValue,
            To = ToValue,
            Duration = new Duration( TimeSpan.FromMilliseconds( ForValue ) )
         };

         FrameworkElement.BeginAnimation( DependencyProperty, animation );
      }
   }

   public class ColorAnimationBuilder : AnimationBuilderBase<Color>
   {
      public ColorAnimationBuilder( FrameworkElement frameworkElement, DependencyProperty dependencyProperty )
         : base( frameworkElement, dependencyProperty )
      {
      }

      public override void Begin()
      {
         var animation = new ColorAnimation
         {
            From = FromValue,
            To = ToValue,
            Duration = new Duration( TimeSpan.FromMilliseconds( ForValue ) )
         };

         FrameworkElement.BeginAnimation( DependencyProperty, animation );
      }

   }
}

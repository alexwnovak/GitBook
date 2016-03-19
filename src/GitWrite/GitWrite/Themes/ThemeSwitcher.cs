using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GitWrite.Themes
{
   public static class ThemeSwitcher
   {
      private class Theme
      {
         public string Name
         {
            get;
         }

         private readonly Dictionary<string, Brush> _colors = new Dictionary<string, Brush>();

         public Theme( string name )
         {
            Name = name;
         }

         public void AddColor( string name, Brush brush ) => _colors[name] = brush;

         public void Apply()
         {
            foreach ( var item in _colors )
            {
               Application.Current.Resources[item.Key] = _colors[item.Key];
            }
         }
      }
   }
}

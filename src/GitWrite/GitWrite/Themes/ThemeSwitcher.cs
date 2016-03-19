﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

      private static readonly string[] _themeFiles =
      {
         "Default",
         "Bold",
      };

      private static readonly List<Theme> _themes = new List<Theme>(); 
      
      public static void Initialize()
      {
         foreach ( string themeFile in _themeFiles )
         {
            var theme = LoadTheme( themeFile );
            _themes.Add( theme );
         }
      }

      public static void SwitchTo( string name )
      {
         var theme = _themes.Single( t => t.Name == name );

         theme.Apply();
      }

      private static Theme LoadTheme( string name )
      {
         var sourceDictionary = new ResourceDictionary
         {
            Source = new Uri( $"/GitWrite;component/Themes/{name}Theme.xaml", UriKind.RelativeOrAbsolute )
         };

         var theme = new Theme( name );

         foreach ( DictionaryEntry resource in sourceDictionary )
         {
            theme.AddColor( (string) resource.Key, (Brush) resource.Value );
         }

         return theme;
      }
   }
}

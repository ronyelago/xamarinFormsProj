﻿using System;
using Xamarin.Forms;
using AppEpi.Droid;
using System.IO;

[assembly: Dependency(typeof(SaveAndLoad_Android))]
namespace AppEpi.Droid
{
    public class SaveAndLoad_Android : ISaveAndLoad
    {
        #region ISaveAndLoad implementation

        public void SaveText(string filename, string text)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, text);
        }


        public string LoadText(string filename)
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                return File.ReadAllText(filePath);
            }
            catch
            {
                return "";
            }
        }

        #endregion
    }
}
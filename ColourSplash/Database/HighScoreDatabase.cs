using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ColourSplash.Models;
using SQLite;
using Environment = System.Environment;

namespace ColourSplash.Database
{
    public static class HighScoreDatabase
    {
        public static string dbFileName = "database.db3";
        public static string dbPath;
        private static SQLiteConnection _db;

        static HighScoreDatabase()
        {
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            dbPath = Path.Combine(libraryPath, dbFileName);
        }

        public static void OpenDatabase()
        {
            if (!System.IO.File.Exists(dbPath))
            {
                File.Create(dbPath);
            }
            _db = new SQLiteConnection(dbPath);
            var result = _db.CreateTable<HighScore>();
            
        }
        
        public static void InsertHighScore(string name, int score)
        {
            if (_db == null) OpenDatabase();

            if (string.IsNullOrEmpty(name) || name.Length > 3)
            {
                return;
            }
            name = name.Substring(0, 3).ToUpper();

            var existingEntries = _db
                .Query<HighScore>($"SELECT * FROM HighSCore WHERE Name LIKE '{name}'")
                .ToList();
            if (existingEntries.Any())
            {
                var currentlyInDb = existingEntries.First();
                if (score < currentlyInDb.Score)
                {
                    currentlyInDb.Score = score;
                    _db.Update(currentlyInDb);
                }
            }
            else { 
                _db.Insert(new HighScore { Name = name, Score = score });
            }
        }
        public static List<HighScore> ReadDatabase()
        {
            var result = _db.Query<HighScore>(
                "SELECT *" +
                "FROM HighScore " +
                "ORDER BY Score");
            return result;
        }

        public static void ClearDatabase()
        {
            _db.DropTable<HighScore>();
            _db.CreateTable<HighScore>();
        }

        public static void CloseConnection()
        {
            _db.Close();
        }
    }
}
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
            //var count = _db.Query<int>("SELECT COUNT(*) FROM HighScore").First();
            //if (count == 0) InsertHighScore("PLT", 21);
            
        }
        
        public static void InsertHighScore(string name, int score)
        {
            if (_db == null) OpenDatabase();
            _db.Insert(new HighScore { Name = name.ToUpper(), Score = score });
        }
        public static List<HighScore> ReadDatabase()
        {
            var result = _db.Query<HighScore>(
                "SELECT *" +
                "FROM HighScore " +
                "ORDER BY Score DESC");
            return result;
        }

        public static void ClearDatabase()
        {
            _db.DropTable<HighScore>();
            _db.CreateTable<HighScore>();
        }

        public static void CreateBogusHighScore()
        {
            if (_db == null) OpenDatabase();
            _db.Insert(new HighScore { Name = "ABC", Score = new Random().Next(100) });

        }

        public static void CloseConnection()
        {
            _db.Close();
        }
    }
}
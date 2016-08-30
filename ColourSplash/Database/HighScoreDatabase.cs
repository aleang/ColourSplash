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
            CheckIfTableExistThenMake();
            _db = new SQLiteConnection(dbPath);
            var result = _db.CreateTable<HighScore>();
            var firstEntry = _db.Get<HighScore>(1);
            if (firstEntry == null)
            {
                InsertHighScore("PLT", 21);
            }
        }

        public static void CheckIfTableExistThenMake()
        {
            if (!System.IO.File.Exists(dbPath)) {
                File.Create(dbPath);
            }
        }

        public static void InsertHighScore(string name, int score)
        {
            if (_db == null) OpenDatabase();
            _db.Insert(new HighScore { Name = name.ToUpper(), Score = score });
        }
        public static List<string> ReadDatabase()
        {
            var highScores = _db.Query<HighScore>(
                "SELECT *" +
                "FROM HighScore " +
                "ORDER BY Score DESC");
            return highScores.Select(h => h.ToString()).ToList();
        }

        public static void ClearDatabase()
        {
            _db.DropTable<HighScore>();
            _db.CreateTable<HighScore>();
        }

        public static void CreateBogusHighScore()
        {
            if (_db == null) OpenDatabase();
            _db.Insert(new HighScore { Name = "ABC", Score = new Random().Next() });

        }

        public static void CloseConnection()
        {
            _db.Close();
        }
    }
}
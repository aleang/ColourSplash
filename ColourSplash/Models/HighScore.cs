using SQLite;

namespace ColourSplash.Models
{
    [Table("HighScore")]
    public class HighScore
    {
       
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(3)]
        public string Name { get; set; }

        public int Score { get; set; }

        public override string ToString()
        {
            return $"{Name} {Score} sec";
        }
    }
}
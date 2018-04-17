using System;
using System.Linq.Expressions;

namespace Dime.Utilities.Expressions.Tests
{    
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public bool IsGolfer { get; set; }
        public PlayerType Type { get; set; }

        public DateTime BirthDate { get; set; }
        public double Height { get; set; }
        public Characteristic Characteristic { get; set; }
    }

    public enum PlayerType
    {
        Golfer = 0,

        Bowler = 1
    }

    [DefaultDisplay(nameof(Characteristic.Category))]
    public class Characteristic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public bool IsGolfer { get; set; }
        public PlayerType Type { get; set; }

        public DateTime BirthDate { get; set; }
        public double Height { get; set; }

        public Stats Stats { get; set; }
    }

    public class Stats
    {
        public int Rank { get; set; }
    }
}
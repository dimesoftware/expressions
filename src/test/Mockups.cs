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
    }

    public enum PlayerType
    {
        Golfer = 0,

        Bowler = 1
    }
}
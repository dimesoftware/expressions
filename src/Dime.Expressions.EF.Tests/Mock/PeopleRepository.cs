using Dime.Expressions.Tests;
using Microsoft.EntityFrameworkCore;

namespace Dime.Expressions.EF.Tests.Mock
{
    public class PeopleRepository
    {
        public CustomerDbContext Create()
        {
            DbContextOptions<CustomerDbContext> options = new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase(databaseName: "people").Options;
            CustomerDbContext context = new(options);
            context.Database.EnsureDeleted();

            List<Person> people = new()
            {
                new Person
                {
                    Id = 1,
                    Type = PlayerType.Bowler,
                    SecondaryType = PlayerType.Golfer,
                    IsGolfer = false,
                    Name = "Jeffrey Lebowski",
                    Height = 185.25,
                    Score = 185.25,
                    Width = 185.25M,
                    Length = 185.25M,
                    BirthDate = new DateTime(1942, 12,4),
                    JoinedNam = new DateTime(2000, 12,4),
                    IsPederast = false,
                    Category = "A",
                    City = "LA",
                    Characteristic = new Characteristic
                    {
                        Type = PlayerType.Bowler,
                        SecondaryType = PlayerType.Golfer,
                        IsGolfer = false,
                        Name = "Jeffrey Lebowski",
                        Height = 185.25,
                        Score = 185.25,
                        Width = 185.25M,
                        Length = 185.25M,
                        BirthDate = new DateTime(1942, 12,4),
                        JoinedNam = new DateTime(2000, 12,4),
                        IsPederast = false,
                        Category = "Well, You Know, That's Just, Like, Your Opinion, Man",
                        City = "LA",
                    }
                },
                new Person
                {
                    Id = 2,
                    Type = PlayerType.Bowler,
                    SecondaryType = PlayerType.Golfer,
                    IsGolfer = false,
                    Name = "Jeffrey Lebowski",
                    Height = 185.25,
                    Score = 185.25,
                    Width = 185.25M,
                    Length =  185.25M,
                    BirthDate = new DateTime(1942, 12,4, 18,5,2),
                    JoinedNam = new DateTime(2000, 12,4, 18,5,2),
                    IsPederast = false,
                    Category = "A",
                    City = "LA",
                    Characteristic = new Characteristic
                    {
                        Type = PlayerType.Bowler,
                        SecondaryType = PlayerType.Golfer,
                        IsGolfer = false,
                        Name = "Jeffrey Lebowski",
                        Height = 185.25,
                        Score = 185.25,
                        Width = 185.25M,
                        Length =  185.25M,
                        BirthDate = new DateTime(1942, 12,4, 18,5,2),
                        JoinedNam = new DateTime(2000, 12,4, 18,5,2),
                        IsPederast = false,
                        Category = "Well, You Know, That's Just, Like, Your Opinion, Man",
                        City = "LA",
                    }
                },
                new Person
                {
                    Id = 3,
                    Type = PlayerType.Golfer,
                    SecondaryType = PlayerType.Bowler,
                    IsGolfer = true,
                    Name = "Walter Sobchak",
                    Height = 193.64,
                    Score = 193.64,
                    Width = 193.64M,
                    Length = 193.64M,
                    BirthDate = new DateTime(1940,1,5),
                    JoinedNam = new DateTime(2000,1,5),
                    IsPederast = true,
                    Category = "A",
                    City = "LA",
                    Characteristic = new Characteristic
                    {
                        Type = PlayerType.Golfer,
                        SecondaryType = PlayerType.Bowler,
                        IsGolfer = true,
                        Name = "Walter Sobchak",
                        Height = 193.64,
                        Score = 193.64,
                        Width = 193.64M,
                        Length = 193.64M,
                        BirthDate = new DateTime(1940,1,5),
                        JoinedNam = new DateTime(2000,1,5),
                        IsPederast = true,
                        Category = "What's a pederast",
                        City = "LA",
                    }
                }
            };

            context.People.AddRange(people);
            context.SaveChanges();
            return context;
        }
    }
}
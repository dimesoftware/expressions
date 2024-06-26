﻿using System;
using System.Linq.Expressions;

namespace Dime.Expressions.Tests
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public bool IsGolfer { get; set; }
        public bool? IsPederast { get; set; }
        public PlayerType Type { get; set; }
        public PlayerType? SecondaryType { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime? JoinedNam { get; set; }
        public double Height { get; set; }
        public double? Score { get; set; }

        public decimal Width { get; set; }
        public decimal? Length { get; set; }
        public Characteristic Characteristic { get; set; }
    }

    public enum PlayerType
    {
        Golfer = 0,

        Bowler = 1
    }

    [DefaultDisplay(nameof(Category))]
    public class Characteristic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public bool IsGolfer { get; set; }
        public bool? IsPederast { get; set; }
        public PlayerType Type { get; set; }
        public PlayerType? SecondaryType { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime? JoinedNam { get; set; }
        public double Height { get; set; }
        public double? Score { get; set; }

        public decimal Width { get; set; }
        public decimal? Length { get; set; }
    }
}
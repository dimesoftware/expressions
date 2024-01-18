using System;
using Dime.Expressions.Tests;
using Microsoft.EntityFrameworkCore;

namespace Dime.Expressions.EF.Tests.Mock
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "AuthorDb");
        }

        public DbSet<Person> People { get; set; }
    }
}
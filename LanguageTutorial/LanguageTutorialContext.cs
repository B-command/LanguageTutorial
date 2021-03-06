﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using LanguageTutorial.DataModel;

namespace LanguageTutorial
{
    public class LanguageTutorialContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Language> Language  { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<WordDictionary> WordDictionary { get; set; }
        public DbSet<WordQueue> WordQueue { get; set; }
        public DbSet<Session> Session { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Chinook Database does not pluralize table names
            modelBuilder.Conventions
                .Remove<PluralizingTableNameConvention>();

        }

    }
}
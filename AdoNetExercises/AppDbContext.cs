using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace AdoNetExercises
{
    public class AppDbContext : DbContext
    {
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<SectionRoster> SectionRosters { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeItem> GradeItems { get; set; }
        public DbSet<GradeType> GradeTypes { get; set; }

        public AppDbContext(): base()
        {

        }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
        }

        public static AppDbContext GetDbContext()
        {
            var builder = new ConfigurationBuilder();

            builder.AddUserSecrets<Program>();

            var config = builder.Build();

            var connectionString = config["ConnectionStrings:SimpleSchool"];

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            return new AppDbContext(options);
        }
    }
}
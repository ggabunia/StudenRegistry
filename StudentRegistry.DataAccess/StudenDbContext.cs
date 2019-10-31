using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using StudentRegistry.Models;

namespace StudentRegistry.DataAccess
{
    public class StudenDbContext : DbContext
    {
        public StudenDbContext(DbContextOptions<StudenDbContext> options) : base(options) { }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>().HasData(
                new Gender
                {
                    ID = 1,
                    GenderName = "მამრობითი"
                },
                new Gender
                {
                    ID = 2,
                    GenderName = "მდედრობითი"
                }
            );
        }

    }


}

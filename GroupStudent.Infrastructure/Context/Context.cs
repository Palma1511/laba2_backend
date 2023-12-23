using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupStudents.Domain;
using Microsoft.EntityFrameworkCore;

namespace GroupStudent.Infrastructure
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            
            Database.EnsureCreated();
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasIndex(g => g.NameGroup).IsUnique();
            modelBuilder.Entity<Group>().HasMany(g => g.Students).WithOne(s => s.Group).OnDelete(DeleteBehavior.SetNull);
        }
    }
}

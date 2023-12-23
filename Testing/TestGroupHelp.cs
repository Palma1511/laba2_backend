using GroupStudent.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GroupStudents.Domain;
using GroupStudent.Infrastructure.Repository;
using GroupStudentInfrastructure.Repository;


namespace Testing
{
    public class TestGroupHelp
    {
        private readonly Context _context;
        public TestGroupHelp()
        {
            var contextOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GroupDbTest").Options;
            _context = new Context(contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var group1 = new GroupStudents.Domain.Group
            {
                NameGroup = "IST-361"
            };
            group1.AddGroup(new Student { Name = "Kirill", Surname = "Antonenko" });
            group1.AddGroup(new Student { Name = "Stepan", Surname = "Stepanov" });

            _context.Groups.Add(group1);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public GroupRepository GroupRepository
        {
            get { return new GroupRepository(_context); }
        }
        public StudentRepository StudentRepository
        {
            get { return new StudentRepository(_context); }
        }

    }
}

using GroupStudent.Infrastructure;
using GroupStudent.Infrastructure.Repository;
using GroupStudentInfrastructure.Repository;
using GroupStudents.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class TestStudentHelp
    {
        private readonly Context _context;

        public TestStudentHelp()
        {
            var contextOptions = new DbContextOptionsBuilder<Context>().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TestStudentDB").Options;

            _context = new Context(contextOptions);


            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var student1 = new Student
            {
                Name = "Kirill",
                Surname = "Antonenko"
            };
            student1.Group = new Group { NameGroup = "ISiT" };
            Group group = new Group { NameGroup = "IST-362" };


            _context.Students.Add(student1);
            _context.Groups.Add(group);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public StudentRepository StudentRepository {
            get { return new StudentRepository(_context); }
        }

        public GroupRepository GroupRepository
        {
            get { return new GroupRepository(_context); }
        }
    }
}

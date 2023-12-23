using GroupStudent.Infrastructure;
using GroupStudents.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupStudentInfrastructure.Repository
{
    public class StudentRepository
    {
        private readonly Context _context;
        public StudentRepository (Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.Include(p => p.Group).OrderBy(p => p.Id).ToListAsync();
        }
        public async Task<Student> GetByIdAsync(Guid id)
        {
            return await _context.Students.Where(p => p.Id == id).Include(p => p.Group).Include(p => p.Surname).FirstOrDefaultAsync();
        }
        public async Task<Student> GetByNameAsync(string name)
        {
            return await _context.Students.Where(p => p.Name == name).Include(p => p.Group).Include(p => p.Surname).FirstOrDefaultAsync();
        }
        public async Task<Student> AddAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }
        public async Task UpdateAsync(Student student)
        {
            var existStud = GetByIdAsync(student.Id).Result;
            if(existStud != null)
            {
                _context.Entry(existStud).CurrentValues.SetValues(student);
                var group = _context.Groups.Where(p => p == student.Group).FirstOrDefaultAsync().Result;
                existStud.Group = group;
                _context.Entry(existStud).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            Student student = await _context.Students.FindAsync(id);
            _context.Remove(student);
            await _context.SaveChangesAsync();
        }
        public void ChangeTrackerClear()
        {
            _context.ChangeTracker.Clear();
        }
    }
}

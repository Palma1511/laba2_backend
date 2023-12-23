using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupStudents.Domain;
using Microsoft.EntityFrameworkCore;

namespace GroupStudent.Infrastructure.Repository
{
    public class GroupRepository
    {
        private readonly Context _context;

        public Context UnityOFWork
        {
            get { return _context; }
        }

        public GroupRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Group>> GetAllAsync()
        {
            return await _context.Groups.Include(s => s.Students).OrderBy(s => s.NameGroup).ToListAsync();
        }
        public async Task<Group> GetByIdAsync(Guid id)
        {
            return await _context.Groups.Where(g => g.Id == id).OrderBy(g => g.NameGroup).Include(g => g.Students).FirstOrDefaultAsync();
        }
        public async Task<Group> GetByNameAsync(string name)
        {
            return await _context.Groups.Where(g => g.NameGroup == name).Include(g => g.Students).FirstOrDefaultAsync();
        }
        public async Task<Group> AddAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }
        public async Task UpdateAsync(Group group)
        {
            var existGroup = GetByIdAsync(group.Id).Result;
            if (existGroup != null )
            {
                _context.Entry(existGroup).CurrentValues.SetValues(group);
                foreach(var studGroup in group.Students)
                {
                    var existStudGroup = existGroup.Students.FirstOrDefault(sg => sg.Id == studGroup.Id);
                    if (existStudGroup == null) {
                        existGroup.Students.Add(studGroup);
                    }
                    else
                    {
                        _context.Entry(existStudGroup).CurrentValues.SetValues(studGroup);
                    }
                }
                foreach (var existStudent in existGroup.Students)
                {
                    if(!group.Students.Any(sg => sg.Id == existStudent.Id))
                    {
                        _context.Remove(existStudent);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            Group group = await _context.Groups.FindAsync(id);
            _context.Remove(group);
            await _context.SaveChangesAsync();
        }

        public void ChangeTrackerClear()
        {
            _context.ChangeTracker.Clear();
        }
    }

}

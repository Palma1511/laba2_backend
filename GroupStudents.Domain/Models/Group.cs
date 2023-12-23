using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupStudents.Domain
{
    public class Group
    {
        public Guid Id { get; set; }

        public string NameGroup { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();

        public void AddGroup(Student student)
        {
            Students.Add(student);
        }
        public void RemoveGroup(int index) 
        {
            Students.RemoveAt(index);
        }

        public int StudentCount {  get { return Students.Count; } }
    }
}

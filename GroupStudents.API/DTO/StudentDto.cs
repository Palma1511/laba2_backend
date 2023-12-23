using GroupStudents.Domain;

namespace GroupStudents.API.DTO
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Groups { get; set; }
    }

}

using GroupStudents.Domain;

namespace GroupStudents.API.DTO
{
    public static class StudentDtoMapper
    {
        public static StudentDto ToDto(Student student)
        {
            var studentDto = new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname
            };
            return studentDto;
        }
        public static List<StudentDto> ToDto(List<Student> students)
        {
            var studentDtos = new List<StudentDto>();
            foreach (var student in students)
            {
                studentDtos.Add(ToDto(student));
            }
            return studentDtos;
        }
        public static Student ToEntity(StudentDto studentDto)
        {
            var student = new Student
            {
                Id = studentDto.Id,
                Name = studentDto.Name,
                Surname = studentDto.Surname,
                
            };
            return student;
        }
    }
}

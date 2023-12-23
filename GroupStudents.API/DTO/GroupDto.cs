namespace GroupStudents.API.DTO
{
    public class GroupDto
    {
        public Guid Id { get; set; }
        public string NameGroup { get; set; } = string.Empty;
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
    }
}

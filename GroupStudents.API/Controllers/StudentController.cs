using GroupStudent.Infrastructure;
using GroupStudent.Infrastructure.Repository;
using GroupStudentInfrastructure.Repository;
using GroupStudents.API.DTO;
using GroupStudents.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GroupStudentsAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly Context _context;
        private readonly StudentRepository _studentRepository;
        private readonly GroupRepository _groupRepository;
        public StudentController(Context context)
        {
            _context = context;
            _studentRepository = new StudentRepository(context);
            _groupRepository = new GroupRepository(context);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudent()
        {
            var students = await _studentRepository.GetAllAsync();
            return StudentDtoMapper.ToDto(students);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDto = StudentDtoMapper.ToDto(student);
            return studentDto;
        }


        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent(StudentDto studentDto)
        {
            var group = await _groupRepository.GetByNameAsync(studentDto.Groups);
            var student = StudentDtoMapper.ToEntity(studentDto);
            student.Group = group;
            var stud = await _studentRepository.AddAsync(student);
            var studDto = StudentDtoMapper.ToDto(stud);
            return CreatedAtAction(nameof(GetStudent), new { id = studDto.Id }, studDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, StudentDto studentDto)
        {
            if (id != studentDto.Id)
            {
                return BadRequest();
            }
            var group = await _groupRepository.GetByNameAsync(studentDto.Groups);
            var student = StudentDtoMapper.ToEntity(studentDto);
            student.Group = group;
            await _studentRepository.UpdateAsync(student);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            await _studentRepository.DeleteAsync(id);
            return NoContent();
        }
    }

}

using GroupStudent.Infrastructure;
using GroupStudent.Infrastructure.Repository;
using GroupStudentInfrastructure.Repository;
using GroupStudents.API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GroupStudents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly Context _context;
        private readonly StudentRepository _studentRepository;
        private readonly GroupRepository _groupRepository;
        public GroupController(Context context)
        {
            _context = context;
            _studentRepository = new StudentRepository(_context);
            _groupRepository = new GroupRepository(_context);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroup()
        {
            var groups = await _groupRepository.GetAllAsync();
            var groupsDto = GroupDtoMapper.ToDto(groups);
            foreach (GroupDto groupDto in groupsDto) 
            {
                foreach (StudentDto studentDto in groupDto.Students)
                {
                    var student = _studentRepository.GetByIdAsync(studentDto.Id);
                    var studDto = StudentDtoMapper.ToDto(student.Result);
                    studentDto.Groups = studDto.Groups;
                }
            }
            return groupsDto;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroup(Guid id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            var groupDto = GroupDtoMapper.ToDto(group);
            foreach (StudentDto studentDto in groupDto.Students)
            {
                var student = _studentRepository.GetByIdAsync(studentDto.Id);
                var studDto = StudentDtoMapper.ToDto(student.Result);
                studentDto.Groups = studDto.Groups;
            }
            return groupDto;
        }
        

        [HttpGet("name/{name}")]
        public async Task<ActionResult<GroupDto>> GetGroupByName(string name)
        {
            var groups = await _groupRepository.GetByNameAsync(name);
            if (groups == null)
            {
                return NotFound();

            }
            var groupDto = GroupDtoMapper.ToDto(groups);
            foreach (StudentDto studentDto in groupDto.Students)
            {
                var student = _studentRepository.GetByIdAsync(studentDto.Id);
                var studDto = StudentDtoMapper.ToDto(student.Result);
                studentDto.Groups = studDto.Groups;
            }
            return groupDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(Guid id, GroupDto groupDto)
        {
            var existGroup = _groupRepository.GetByNameAsync(groupDto.NameGroup).Result;
            if (id != groupDto.Id)
            {
                return BadRequest();
            }
            if (existGroup != null) {
                if (existGroup.Id != groupDto.Id) {
                    return BadRequest();
                }
            }
            var group = GroupDtoMapper.ToEntity(groupDto);
            await _groupRepository.UpdateAsync(group);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GroupDto>> PostGroup(GroupDto groupDto)
        {
            var existGroup = _groupRepository.GetByNameAsync(groupDto.NameGroup).Result;
            if (existGroup == null)
            {
                return BadRequest();
            }
            else {
                var group = GroupDtoMapper.ToEntity(groupDto);
                var group2 = await _groupRepository.AddAsync(group);
                var groupDto2 = GroupDtoMapper.ToDto(group2);
                return CreatedAtAction("GetPerson", new { id = groupDto2.Id }, groupDto2);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) { return NotFound();}
            await _groupRepository.DeleteAsync(id);
            return NoContent();
        }
        private bool GroupExist(Guid id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}

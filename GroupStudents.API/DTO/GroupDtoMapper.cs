using GroupStudents.Domain;

namespace GroupStudents.API.DTO
{
    public static class GroupDtoMapper
    {
        public static GroupDto ToDto(Group group)
        {
            var groupDto = new GroupDto()
            {
                Id = group.Id,
                NameGroup = group.NameGroup,
            };
            foreach(var student in group.Students)
            {
                groupDto.Students.Add(StudentDtoMapper.ToDto(student));
            }
            return groupDto;
        }
        public static Group ToEntity(GroupDto groupDto)
        {
            var group = new Group
            {
                Id = groupDto.Id,
                NameGroup = groupDto.NameGroup,
            };
            foreach(var studDto in groupDto.Students)
            {
                var stud = StudentDtoMapper.ToEntity(studDto);
                stud.Group = group;
  
                group.AddGroup(stud);
            }
            return group;
        }
        public static List<GroupDto> ToDto(List<Group> groups)
        {
            var groupDtos = new List<GroupDto>();
            foreach(var group in groups)
            {
                groupDtos.Add(ToDto(group));
            }
            return groupDtos;
        }
    }
}

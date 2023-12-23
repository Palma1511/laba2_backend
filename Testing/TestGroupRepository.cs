using GroupStudents.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Testing
{
    public class TestGroupRepository
    {
        //Проверка работы базы данных
        [Fact]
        public void VoidTest()
        {
            var testHelper = new TestGroupHelp();
            var groupRepository = testHelper.GroupRepository;

            Assert.Equal(1, 1);
        }

        [Fact]

        public async void TestAdd()
        {
            var testHelper = new TestGroupHelp();
            var groupRepository = testHelper.GroupRepository;
            var group = new Group { NameGroup = "AAAAAAAAA14" };
            group.Id = Guid.NewGuid();

            var add_group = await groupRepository.AddAsync(group);
            
            groupRepository.ChangeTrackerClear();

            Assert.True(groupRepository.GetAllAsync().Result.Count == 2);
            Assert.Equal("AAAAA15", groupRepository.GetByNameAsync("AAAAA15").Result.NameGroup);
            Assert.Equal("AAAAAAAAA14", groupRepository.GetByNameAsync("AAAAAAAAA14").Result.NameGroup);
            Assert.Equal(2, groupRepository.GetByNameAsync("AAAAA15").Result.StudentCount);
        }

        [Fact]
        public void TestUpdateAdd()
        {
            var testHelper = new TestGroupHelp();
            var groupRepository = testHelper.GroupRepository;
            var group = groupRepository.GetByNameAsync("AAAAA15").Result;
            
            groupRepository.ChangeTrackerClear();
            group.NameGroup = "Носочки";
            var student = new Student { Name = "Aloxa", Surname = "Pofig" };
            group.AddGroup(student);

            groupRepository.UpdateAsync(group).Wait();

            Assert.Equal("AAAAAAAAA14", groupRepository.GetByNameAsync("AAAAAAAAA14").Result.NameGroup);
            Assert.Equal(3, groupRepository.GetByNameAsync("AAAAAAAAA14").Result.StudentCount);
        }
        [Fact]
        public void TestUpdateToyDelete()
        {
            var testHelper = new TestGroupHelp();
            var groupRepository = testHelper.GroupRepository;
            var group = groupRepository.GetByNameAsync("AAAAA15").Result;

            groupRepository.ChangeTrackerClear();
            group.RemoveGroup(0);

            groupRepository.UpdateAsync(group).Wait();

            Assert.Equal(1, groupRepository.GetByNameAsync("AAAAA15").Result.StudentCount);
        }
        [Fact]
        public void TestUpdateDelete()
        {
            var testHelper = new TestGroupHelp();
            var groupRepository = testHelper.GroupRepository;
            var studentRepository = testHelper.StudentRepository;
            var group = groupRepository.GetByNameAsync("AAAAA15").Result;

            groupRepository.ChangeTrackerClear();

            groupRepository.DeleteAsync(group.Id).Wait();

            Assert.Equal(null, groupRepository.GetByNameAsync("AAAAA15").Result);
            Assert.Equal(null, studentRepository.GetByIdAsync(group.Students[0].Id).Result.Group);
        }
    }
}

using GroupStudent.Infrastructure.Repository;
using GroupStudents.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Testing
{
    public class TestStudentRepository
    {
        [Fact]
        //Тест, проверяющий, что база данных создалась
        public void VoidTest()
        {
            var testHelper = new TestStudentHelp();
            var studentRepository = testHelper.StudentRepository;

            Assert.Equal(1, 1);
        }

        [Fact]
        public async void TestAdd()
        {
            var testHelper = new TestStudentHelp();
            var studentRepository = testHelper.StudentRepository;
            var student = new Student { Name = "Stepan", Surname = "Stepanov" };
            student.Id = Guid.NewGuid();

            var add_toy = await studentRepository.AddAsync(student);

            studentRepository.ChangeTrackerClear();

            Assert.True(studentRepository.GetAllAsync().Result.Count == 2);
            Assert.Equal("Stepan", studentRepository.GetByIdAsync(student.Id).Result.Name);
            Assert.Equal("Kirill", studentRepository.GetByNameAsync("Kirill").Result.Name);
            Assert.Equal("Stepan", studentRepository.GetByNameAsync("Stepan").Result.Name);
        }
        [Fact]
        public void TestUpdateAdd()
        {
            var testHelper = new TestStudentHelp();
            var studentRepository = testHelper.StudentRepository;
            var groupRepository = testHelper.GroupRepository;
            var student = studentRepository.GetByNameAsync("Kirill").Result;
            //Запрещаем отслеживание сущностей (разрываем связи с БД)
            studentRepository.ChangeTrackerClear();
            student.Name = "Kirill";
            student.Group = groupRepository.GetByNameAsync("IST-362").Result;

            studentRepository.UpdateAsync(student).Wait();

            Assert.Equal("Kirill", studentRepository.GetByNameAsync("Kirill").Result.Name);
            Assert.Equal("IST-362", studentRepository.GetByNameAsync("Kirill").Result.Group.NameGroup);
        }
        [Fact]
        public void TestUpdateDelete()
        {
            var testHelper = new TestStudentHelp();
            var studentRepository = testHelper.StudentRepository;
            var student = studentRepository.GetByNameAsync("Kirill").Result;
            //Запрещаем отслеживание сущностей (разрываем связи с БД)
            studentRepository.ChangeTrackerClear();

            studentRepository.DeleteAsync(student.Id).Wait();

            Assert.Equal(null, studentRepository.GetByIdAsync(student.Id).Result);
        }
    }
}

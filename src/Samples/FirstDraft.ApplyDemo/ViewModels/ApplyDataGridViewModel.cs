using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FirstDraft.ApplyDemo.ViewModels
{
    public class ApplyDataGridViewModel : ObservableObject
    {
        public ObservableCollection<Student> Students { get; set; }

        public ApplyDataGridViewModel()
        {
            // 初始化数据
            Students = new ObservableCollection<Student>
        {
            new Student { StudentId = 202409001, Name = "Alice", RegistrationDate = DateTime.Now.AddMonths(-1), Email = "alice@example.com", Course = "Mathematics" },
            new Student { StudentId = 202409002, Name = "Bob", RegistrationDate = DateTime.Now.AddMonths(-2), Email = "bob@example.com", Course = "Physics" },
            new Student { StudentId = 202409003, Name = "Charlie", RegistrationDate = DateTime.Now.AddMonths(-3), Email = "charlie@example.com", Course = "Computer Science" },
            new Student { StudentId = 202409004, Name = "Mel", RegistrationDate = DateTime.Now.AddMonths(-11), Email = "mel@example.com", Course = "Computer Science" },
            new Student { StudentId = 202409005, Name = "David", RegistrationDate = DateTime.Now.AddMonths(-6), Email = "david@example.com", Course = "Engineering" },
            new Student { StudentId = 202409006, Name = "Eva", RegistrationDate = DateTime.Now.AddMonths(-8), Email = "eva@example.com", Course = "Mathematics" },
            new Student { StudentId = 202409007, Name = "Frank", RegistrationDate = DateTime.Now.AddMonths(-5), Email = "frank@example.com", Course = "Physics" },
            new Student { StudentId = 202409008, Name = "Grace", RegistrationDate = DateTime.Now.AddMonths(-9), Email = "grace@example.com", Course = "Computer Science" },
            new Student { StudentId = 202409009, Name = "Hank", RegistrationDate = DateTime.Now.AddMonths(-12), Email = "hank@example.com", Course = "Engineering" },
            new Student { StudentId = 202409010, Name = "Ivy", RegistrationDate = DateTime.Now.AddMonths(-4), Email = "ivy@example.com", Course = "Mathematics" },
            new Student { StudentId = 202409011, Name = "Jack", RegistrationDate = DateTime.Now.AddMonths(-7), Email = "jack@example.com", Course = "Physics" },
            new Student { StudentId = 202409012, Name = "Kathy", RegistrationDate = DateTime.Now.AddMonths(-10), Email = "kathy@example.com", Course = "Computer Science" },
            new Student { StudentId = 202409013, Name = "Leo", RegistrationDate = DateTime.Now.AddMonths(-2), Email = "leo@example.com", Course = "Engineering" },
            new Student { StudentId = 202409014, Name = "Mia", RegistrationDate = DateTime.Now.AddMonths(-3), Email = "mia@example.com", Course = "Mathematics" },
            new Student { StudentId = 202409015, Name = "Nina", RegistrationDate = DateTime.Now.AddMonths(-6), Email = "nina@example.com", Course = "Physics" },
            new Student { StudentId = 202409016, Name = "Oscar", RegistrationDate = DateTime.Now.AddMonths(-8), Email = "oscar@example.com", Course = "Computer Science" },
            new Student { StudentId = 202409017, Name = "Paul", RegistrationDate = DateTime.Now.AddMonths(-12), Email = "paul@example.com", Course = "Engineering" },
            new Student { StudentId = 202409018, Name = "Quinn", RegistrationDate = DateTime.Now.AddMonths(-5), Email = "quinn@example.com", Course = "Mathematics" },
            new Student { StudentId = 202409019, Name = "Rachel", RegistrationDate = DateTime.Now.AddMonths(-7), Email = "rachel@example.com", Course = "Physics" },
            new Student { StudentId = 202409020, Name = "Steve", RegistrationDate = DateTime.Now.AddMonths(-9), Email = "steve@example.com", Course = "Computer Science" }
        };

        }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Email { get; set; }
        public string Course { get; set; }
    }
}

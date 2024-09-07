using System;
using System.Collections.Generic;
using BasicFunctions.Models;    

namespace BasicFunctions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var repository = new StudentRepository();

            var studentConnected = repository.GetAllConnected();
            Console.WriteLine("Connected Model:");
            PrintStudents(studentConnected);
            Console.WriteLine("-----\n");

            var studentDisconnected = repository.GetAllDisconnected();
            Console.WriteLine("Disconnected Model:");
            PrintStudents(studentDisconnected);
            Console.WriteLine("-----\n");

            var studentProcedure = repository.GetAllStoredViaProcedure();
            Console.WriteLine("Fetched via a Stored procedure:");
            PrintStudents(studentProcedure);
            Console.WriteLine("-----\n");

            var student = repository.Get(1);
            Console.WriteLine("Fetched by id:");
            PrintStudent(student);
            Console.WriteLine("-----\n");

            var facultyName = repository.GetFacultyNameViaFucntion(student.Id);
            Console.WriteLine("Student faculty name:");
            Console.WriteLine(facultyName);
            Console.WriteLine("-----\n");

            var newStudent = new StudentModel()
            {
                FirstName = "Edward",
                LastName = "Brown",
                Email = "m.f@protonmail.edu",
                PhoneNumber = "1-683-204-3142",
                AverageGrade = 9.2f,
                FacultyId = 1
            };
            var newStudentId = repository.Add(newStudent);
            Console.WriteLine($"New student added: id {newStudentId}");
            Console.WriteLine("-----\n");
        }

        private static void PrintStudents(List<StudentModel> students)
        {
            foreach (var student in students)
            {
                PrintStudent(student);
            }
        }

        private static void PrintStudent(StudentModel student)
        {
            Console.WriteLine($"{student.Id}, {student.FirstName}, {student.LastName}," +
                              $"{student.Email}, {student.PhoneNumber}, {student.AverageGrade}," +
                              $"{(student.FacultyId.HasValue ? student.FacultyId : "N/A")}");
        }
    }
}

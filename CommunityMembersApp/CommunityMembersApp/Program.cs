using System;
using System.Collections.Generic;
using CommunityMembersApp.Models; // Adjust if your namespace is different

namespace CommunityMembersApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== COMMUNITY MEMBERS DEMO ===\n");

            // Create instances of each class
            var admin = new Administrative
            {
                Id = 1,
                FullName = "Ana Torres",
                Email = "ana@campus.edu",
                BaseSalary = 1800m,
                //Subject = "Math",
                //Area = "Academics"
            };

            var instructor = new Instructor
            {
                Id = 2,
                FullName = "Carlos Pérez",
                Email = "carlos@campus.edu",
                BaseSalary = 1200m,
                Subject = "History",
                Grade = "High School"
            };

            var adminStaff = new Administrative
            {
                Id = 3,
                FullName = "María Gómez",
                Email = "maria@campus.edu",
                BaseSalary = 1000m,
                Department = "Human Resources"
            };

            var student = new Student
            {
                Id = 4,
                FullName = "Luis Martínez",
                Email = "luis@campus.edu",
                StudentNumber = "STU-001",
                EnrollmentDate = new DateTime(2024, 9, 1)
            };

            var alumni = new Alumni
            {
                Id = 5,
                FullName = "Laura Díaz",
                Email = "laura@campus.edu",
                GraduationDate = new DateTime(2022, 6, 30)
            };

            // Store all in a single list (demonstrates polymorphism)
            List<CommunityMember> members = new() { admin, instructor, adminStaff, student, alumni };

            // Display all members
            Console.WriteLine("=== All Members ===");
            foreach (var m in members)
            {
                Console.WriteLine(m); // Calls overridden ToString()
            }

            // Polymorphism in action
            Console.WriteLine("\n=== Roles by Type ===");
            foreach (var m in members)
            {
                Console.WriteLine($"{m.FullName} is a {m.GetRole()}");
            }

            // Test specific behavior
            Console.WriteLine("\n=== Payroll Calculation for Employees ===");
            foreach (var e in members)
            {
                if (e is Employee emp)
                {
                    Console.WriteLine($"{emp.FullName,-15} | Role: {emp.GetRole(),-13} | Salary: {emp.CalculateSalary():C}");
                }
            }

            // Example of updating data manually
            Console.WriteLine("\nUpdating Instructor’s BaseSalary...");
            instructor.BaseSalary += 300m;
            Console.WriteLine($"New salary for {instructor.FullName}: {instructor.CalculateSalary():C}");

            Console.WriteLine("\n=== Final Summary ===");
            foreach (var m in members)
            {
                Console.WriteLine(m.ToString());
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}

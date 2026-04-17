using BlindMatchPAS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Only seed if Users table is empty
            if (await context.Users.AnyAsync())
                return;

            // ===========================
            // Create Admin Account
            // ===========================
            var adminUser = new User
            {
                FullName = "System Admin",
                Email = "admin@admin.nsbm.ac.lk",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            // ===========================
            // Create Supervisor Account
            // ===========================
            var supervisorUser = new User
            {
                FullName = "Dr. Sample Supervisor",
                Email = "supervisor@supervisor.nsbm.ac.lk",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Super123"),
                Role = "Supervisor",
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(supervisorUser);
            await context.SaveChangesAsync();

            // Create supervisor profile
            var supervisorProfile = new SupervisorProfile
            {
                UserId = supervisorUser.Id,
                Department = "Faculty of Computing",
                MaxQuota = 5,
                PreferredAreas = "AI,Software"
            };
            context.SupervisorProfiles.Add(supervisorProfile);
            await context.SaveChangesAsync();

            // ===========================
            // Create Student Account
            // ===========================
            var studentUser = new User
            {
                FullName = "Sample Student",
                Email = "student@student.nsbm.ac.lk",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student123"),
                Role = "Student",
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(studentUser);
            await context.SaveChangesAsync();

            // Create student profile
            var studentProfile = new StudentProfile
            {
                UserId = studentUser.Id
            };
            context.StudentProfiles.Add(studentProfile);
            await context.SaveChangesAsync();

            // ===========================
            // Create Sample Projects
            // ===========================
            var projects = new List<Project>
            {
                new Project
                {
                    Title = "AI Traffic Optimization System",
                    Abstract = "Utilizes neural networks to predict and optimize urban traffic flow patterns in real time. The system will collect data from sensors and cameras to reduce congestion.",
                    TechnicalStack = "Python, TensorFlow, ASP.NET Core",
                    ResearchArea = "AI",
                    Status = "Pending",
                    SubmittedAt = DateTime.UtcNow,
                    StudentProfileId = studentProfile.Id
                },
                new Project
                {
                    Title = "Hotel Management System",
                    Abstract = "A comprehensive desktop application for managing hotel bookings, room allocations, and customer records with real-time availability tracking.",
                    TechnicalStack = "Java, MySQL, JavaFX",
                    ResearchArea = "Java",
                    Status = "Pending",
                    SubmittedAt = DateTime.UtcNow,
                    StudentProfileId = studentProfile.Id
                },
                new Project
                {
                    Title = "E-Commerce Web Portal",
                    Abstract = "A scalable software architecture for online retail with product management, cart system, payment gateway integration and order tracking.",
                    TechnicalStack = "ASP.NET Core, SQL Server, Bootstrap",
                    ResearchArea = "Software",
                    Status = "Pending",
                    SubmittedAt = DateTime.UtcNow,
                    StudentProfileId = studentProfile.Id
                },
                new Project
                {
                    Title = "Secure Banking API",
                    Abstract = "Enterprise-level REST API for handling secure financial transactions with JWT authentication, rate limiting, and audit logging.",
                    TechnicalStack = "C#, ASP.NET Core, SQL Server",
                    ResearchArea = "C#",
                    Status = "Pending",
                    SubmittedAt = DateTime.UtcNow,
                    StudentProfileId = studentProfile.Id
                }
            };

            context.Projects.AddRange(projects);
            await context.SaveChangesAsync();
        }
    }
}
using BlindMatchPAS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlindMatchPAS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<StudentProfile> StudentProfiles { get; set; } = null!;
        public DbSet<SupervisorProfile> SupervisorProfiles { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Match> Matches { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.StudentProfile)
                .WithOne(s => s.User)
                .HasForeignKey<StudentProfile>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.SupervisorProfile)
                .WithOne(s => s.User)
                .HasForeignKey<SupervisorProfile>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentProfile>()
                .HasMany(s => s.Projects)
                .WithOne(p => p.StudentProfile)
                .HasForeignKey(p => p.StudentProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Project)
                .WithOne(p => p.Match)
                .HasForeignKey<Match>(m => m.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.SupervisorProfile)
                .WithMany(s => s.Matches)
                .HasForeignKey(m => m.SupervisorProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
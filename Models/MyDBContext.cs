using Microsoft.EntityFrameworkCore;

namespace NetCoreExamProject.Models
{
    public class MyDBContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Filename=./NetCoreExamProject.db");
        public MyDBContext()
        {
            this.Database.EnsureCreated();
            this.Database.Migrate();
        }
    }
}

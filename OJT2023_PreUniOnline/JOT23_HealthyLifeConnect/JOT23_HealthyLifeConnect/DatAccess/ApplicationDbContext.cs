using JOT23_Pre2UniOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace JOT23_Pre2UniOnline.DatAccess
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Lecturer> Lecturers { get; set; }
		public DbSet<Student> Students { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Thiết lập mối quan hệ 1:1 giữa Account và Lecturer
			modelBuilder.Entity<Account>()
				.HasOne(a => a.Lecturer)
				.WithOne(l => l.Account)
				.HasForeignKey<Lecturer>(l => l.IDAccount);

			// Thiết lập mối quan hệ 1:1 giữa Account và Student
			modelBuilder.Entity<Account>()
				.HasOne(a => a.Student)
				.WithOne(s => s.Account)
				.HasForeignKey<Student>(s => s.IDAccount);
		}
	}
}

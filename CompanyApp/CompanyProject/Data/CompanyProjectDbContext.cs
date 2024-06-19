using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Data
{
    public class CompanyProjectDbContext : DbContext
    {
        public CompanyProjectDbContext() { }

        public CompanyProjectDbContext(DbContextOptions<CompanyProjectDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<EmployeeTask> EmployeeTasks { get; set; }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync();
            Console.WriteLine($"Saved {result} changes to the database.");
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Greek_CI_AI");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("EMPLOYEES");
                entity.HasIndex(e => e.Email, "UQ_EMAIL").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Firstname)
                    .HasMaxLength(100).HasColumnName("FIRSTNAME");
                entity.Property(e => e.Lastname)
                    .HasMaxLength(100).HasColumnName("LASTNAME");
                entity.Property(e => e.Email)
                    .HasMaxLength(200).HasColumnName("EMAIL").IsRequired();
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10).HasColumnName("PHONE_NUMBER");
                entity.HasOne<User>()
                        .WithMany()
                        .HasForeignKey(e => e.UserId)
                        .HasConstraintName("FK_EMPLOYEES_USERS")
                        .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<EmployeeTask>(entity =>
            {
                entity.ToTable("EMPLOYEES_X_TASKS");

                entity.HasKey(k => new { k.TaskId, k.EmployeeId });
                entity.Property(e => e.TaskId).HasColumnName("TASK_ID");
                entity.Property(e => e.EmployeeId).HasColumnName("EMPLOYEE_ID");
                entity.HasOne(ep => ep.Employee)
                    .WithMany(e => e.EmployeeTasks)
                    .HasForeignKey(ep => ep.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EMPLOYEES__EMPLO");
                entity.HasOne(ep => ep.Task)
                    .WithMany(p => p.EmployeeTasks)
                    .HasForeignKey(ep => ep.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EMPLOYEES__PROJ");
            });


            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("TASKS");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Title)
                    .HasMaxLength(100).HasColumnName("TITLE").IsRequired();
                entity.Property(e => e.Description)
                    .HasMaxLength(300).HasColumnName("DESCRIPTION");
                entity.Property(e => e.Deadline)
                    .HasColumnName("DEADLINE").IsRequired();
                entity.HasMany(e => e.Employees)
                    .WithMany(p => p.Tasks)
                    .UsingEntity<EmployeeTask>(t => t.ToTable("EMPLOYEES_X_TASKS"));
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS");
                entity.HasIndex(e => e.Username, "UQ_USERNAME").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Username)
                    .HasMaxLength(100).HasColumnName("USERNAME").IsRequired();
                entity.Property(e => e.Password)
                    .HasMaxLength(512).HasColumnName("PASSWORD").IsRequired();
            });
        }

    }


}

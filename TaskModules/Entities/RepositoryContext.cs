using Microsoft.EntityFrameworkCore;
using TaskModules.Entities.Models;

namespace TaskModules.Entities
{
    public class RepositoryContext: DbContext
    {
        public RepositoryContext(DbContextOptions options): base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Department> Department { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasOne(d=>d.ParentDept)
                .WithMany(d=>d.SubDepartments)
                .HasForeignKey(d=>d.ParentDeptID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);            
        }

        
    }
}

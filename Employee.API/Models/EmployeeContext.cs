using Microsoft.EntityFrameworkCore;

namespace Employee.API.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        { }
        
        public DbSet<EmployeeDetails> Details { get; set; }
        public DbSet<DesignationDetails> Designation { get; set; }
        public DbSet<DepartmentDetails> Departments { get; set; }
    }
}

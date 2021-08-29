using System.ComponentModel.DataAnnotations;

namespace Employee.API.Models
{
    public class EmployeeDetails
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Department { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; }

    }
}

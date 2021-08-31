using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.Models
{
    public class DesignationDetails
    {
        [Key]
        public int Id { get; set; }
        public string DesignationName { get; set; }
    }
}

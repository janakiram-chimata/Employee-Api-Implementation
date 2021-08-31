using Employee.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var designation = _context.Designation;
            var department = _context.Departments;
            var details = await _context.Details.Select(x => new {
                Name = x.Name,
                Email = x.Email,
                Location = x.Location,
                Department = x.Department,
                IsAvailable = x.IsAvailable,
                Id = x.Id,
                DesignationId = x.DesignationId
            }).ToArrayAsync();

            var employeeQuery =
                from e in details 
                join d in designation on e.DesignationId equals d.Id
                join de in department on e.Department equals de.Id
                select new { Id = e.Id, Name = e.Name, Email = e.Email , Role = d.DesignationName , Department = de.DepartmentName };

            return Ok(employeeQuery);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var designation = _context.Designation;
            var department = _context.Departments;
            var details = _context.Details;

            var detail = await _context.Details.FindAsync(id);
            if (detail == null)
            {
                return NotFound();
            }
                var employeeQuery =
                from e in details where e.Id == detail.Id
                join d in designation on e.DesignationId equals d.Id 
                join de in department on e.Department equals de.Id 
                select new { Id = e.Id, Name = e.Name, Email = e.Email, Role = d.DesignationName, Department = de.DepartmentName }
                ;

                return Ok(employeeQuery);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDetails>> PostEmployee(EmployeeDetails details)
        {
            _context.Details.Add(details);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEmployee", new { id = details.Id }, new { Id = details.Id , Name = details.Name});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id , [FromBody] EmployeeDetails details)
        {
            if(id != details.Id)
            {
                return BadRequest();
            }
            _context.Entry(details).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(_context.Details.Find(id) == null)
                {
                    return NotFound();
                }

                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeDetails>> DeleteEmployee(int id)
        {
            var details = await _context.Details.FindAsync(id);
            if (details == null)
            {
                return NotFound();
            }
            _context.Details.Remove(details);
            await _context.SaveChangesAsync();
            return details;
        }

    }
}
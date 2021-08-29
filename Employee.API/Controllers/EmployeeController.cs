using Employee.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return Ok(await _context.Details.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var detail = await _context.Details.FindAsync(id);

            if(detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDetails>> PostEmployee(EmployeeDetails details)
        {
            _context.Details.Add(details);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEmployee", new { id = details.Id }, details);
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
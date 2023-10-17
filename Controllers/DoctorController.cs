using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorApi.Data;
using DoctorApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace DoctorApi.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public DoctorController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorModel>>> GetDoctors()
        {
          if (_context.Doctors == null)
          {
              return NotFound();
          }
            return await _context.Doctors.ToListAsync();
        }

        // GET: api/doctor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorModel>> GetDoctorModel(int id)
        {
          if (_context.Doctors == null)
          {
              return NotFound();
          }
            var doctorModel = await _context.Doctors.FindAsync(id);

            if (doctorModel == null)
            {
                return NotFound();
            }

            return doctorModel;
        }

        // PUT: api/doctor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctorModel(int id, DoctorModel doctorModel)
        {
            if (id != doctorModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(doctorModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/doctor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DoctorModel>> PostDoctorModel(DoctorModel doctorModel)
        {
          if (_context.Doctors == null)
          {
              return Problem("Entity set 'DatabaseContext.Doctors'  is null.");
          }
            _context.Doctors.Add(doctorModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctorModel", new { id = doctorModel.Id }, doctorModel);
        }

        // DELETE: api/doctor/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorModel(int id)
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            var doctorModel = await _context.Doctors.FindAsync(id);
            if (doctorModel == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctorModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorModelExists(int id)
        {
            return (_context.Doctors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

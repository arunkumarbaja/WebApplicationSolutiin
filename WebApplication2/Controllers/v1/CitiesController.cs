using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DatabaseContext;
using WebApplication2.Models;

namespace WebApplication2.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        /// <summary>
        /// to get list of cities (including city name and city id ) form cities table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
         //[Produces("application/xml")]
        public async Task<ActionResult<IEnumerable<City>>> GetCities() // Action Result return type is used when you are sure that yoy are returning the an object
        {
            return await _context.Cities.ToListAsync();
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(Guid id, [Bind(nameof(city.CityName), nameof(city.CityId))] City city) // IAction Result return type is used to return various result types other than object result 
        {
            if (id != city.CityId)
            {
                return Problem(detail: "Invalid CItyId", statusCode: 400, title: "City Search"); // to return model validation errors to the client application 

                // return BadRequest();//HTTP 400
            }

            // _context.Entry(city).State = EntityState.Modified;// this method forces us to update all values of an obj

            var resultCity = await _context.Cities.FindAsync(id);
            if (resultCity == null)
            {
                return NotFound();
            }

            resultCity.CityName = city.CityName;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) // if the city is updated already by another user parallelly
            {
                if (!CityExists(id))
                {
                    return NotFound();//HTTP 400
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCity", new { id = resultCity.CityId }, city);
        }

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<ActionResult<City>> PostCity(Guid id, [Bind(nameof(city.CityName), nameof(city.CityId))] City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.CityId }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(Guid id)
        {
            return _context.Cities.Any(e => e.CityId == id);
        }
    }
}

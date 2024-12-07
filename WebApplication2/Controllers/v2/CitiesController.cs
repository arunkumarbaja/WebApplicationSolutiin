using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DatabaseContext;
using WebApplication2.Models;

namespace WebApplication2.Controllers.v2
{
    [ApiVersion("2.0")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<String?>>> GetCities() // Action Result return type is used when you are sure that yoy are returning the an object
        {
            return await _context.Cities.OrderBy(temp => temp.CityName).Select(temp=>temp.CityName).ToListAsync();
        }
    }
}

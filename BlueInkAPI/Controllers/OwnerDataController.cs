using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueInk.API.Data;
using BlueInk.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueInk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerDataController : ControllerBase
    {
        private readonly BlueInkDbContext _context;
        public OwnerDataController(BlueInkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetOwnerData()
        {
            var socialLinks = _context.OwnerData.FirstOrDefaultAsync();

            return Ok(socialLinks);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwnerData([FromBody] OwnerData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model not valid.");
            }

            if (!_context.OwnerData.Any())
            {
                return NotFound();
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(model);
        }

    }
}
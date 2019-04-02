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
    public class ProjectsController : ControllerBase
    {
        private readonly BlueInkDbContext _context;
        public ProjectsController(BlueInkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProjects()
        {
            var projects = _context.Projects.ToList();

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject([FromRoute]int id)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model not valid.");
            }

            model.Id = 0; // to prevent an id from being supplied

            var result = _context.Projects.Add(model).Entity;
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject([FromRoute]int id, [FromBody] Project model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model not valid.");
            }

            if (!_context.Projects.Any(p => p.Id == id))
            {
                return NotFound();
            }

            model.Id = id; // prevent updating the wrong entity

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute]int id)
        {
            var entity = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            _context.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
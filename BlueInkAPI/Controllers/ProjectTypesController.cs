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
    [Route("api/projectTypes")]
    [ApiController]
    public class ProjectTypesController : ControllerBase
    {
        private readonly BlueInkDbContext _context;
        public ProjectTypesController(BlueInkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProjectTypes()
        {
            var projectTypes = _context.ProjectTypes.ToList();

            return Ok(projectTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectType([FromRoute]int id)
        {
            var projectType = await _context.ProjectTypes.SingleOrDefaultAsync(p => p.Id == id);

            if (projectType == null)
            {
                return NotFound();
            }

            return Ok(projectType);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProjectType([FromBody] ProjectType model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model not valid.");
            }

            model.Id = 0; // to prevent an id from being supplied

            var result = _context.ProjectTypes.Add(model).Entity;
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectType([FromRoute]int id, [FromBody] ProjectType model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model not valid.");
            }

            if (!_context.ProjectTypes.Any(p => p.Id == id))
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
        public async Task<IActionResult> DeleteProjectType([FromRoute]int id)
        {
            var entity = _context.ProjectTypes.SingleOrDefault(p => p.Id == id);

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
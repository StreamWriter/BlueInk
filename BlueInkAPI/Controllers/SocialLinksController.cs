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
    public class SocialLinksController : ControllerBase
    {
        private readonly BlueInkDbContext _context;
        public SocialLinksController(BlueInkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSocialLinks()
        {
            var socialLinks = _context.SocialLinks.ToList();

            return Ok(socialLinks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSocialLink([FromRoute]int id)
        {
            var socialLink = await _context.SocialLinks.SingleOrDefaultAsync(p => p.Id == id);

            if (socialLink == null)
            {
                return NotFound();
            }

            return Ok(socialLink);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSocialLink([FromBody] SocialLink model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model not valid.");
            }

            model.Id = 0; // to prevent an id from being supplied

            var result = _context.SocialLinks.Add(model).Entity;
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSocialLink([FromRoute]int id, [FromBody] SocialLink model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model not valid.");
            }

            if (!_context.SocialLinks.Any(p => p.Id == id))
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
        public async Task<IActionResult> DeleteSocialLink([FromRoute]int id)
        {
            var entity = _context.SocialLinks.SingleOrDefault(p => p.Id == id);

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
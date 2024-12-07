using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Interfaces.Services;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomClassController : ControllerBase
    {
        private readonly IRoomClassService _service;

        public RoomClassController(IRoomClassService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roomClasses = await _service.GetAllAsync();
            return Ok(roomClasses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var roomClass = await _service.GetByIdAsync(id);
            if (roomClass is null) return NotFound();
            return Ok(roomClass);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomClass roomClass)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddAsync(roomClass);
            return CreatedAtAction(nameof(GetById), new { id = roomClass.Id }, roomClass);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomClass roomClass)
        {
            if (id != roomClass.Id) return BadRequest("ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _service.UpdateAsync(roomClass);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
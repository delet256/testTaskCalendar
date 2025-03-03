using Calendar.Models;
using Calendar.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Calendar.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _noteService.GetAllNotesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _noteService.GetNoteByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(Note note)
        {
            await _noteService.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Note note)
        {
            if (id != note.Id) return BadRequest();
            await _noteService.UpdateNoteAsync(note);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }

        [HttpGet("odata")]
        public IActionResult GetOData()
        {
            return Ok(_noteService.GetNotesQueryable());
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportToCsv()
        {
            var csvData = await _noteService.ExportToCsvAsync();
            var bytes = Encoding.UTF8.GetBytes(csvData);
            return File(bytes, "text/csv", "notes.csv");
        }
    }

}

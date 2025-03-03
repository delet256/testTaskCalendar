using Calendar.Models;

namespace Calendar.Services.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<Note> GetNoteByIdAsync(int id);
        Task CreateNoteAsync(Note note);
        Task UpdateNoteAsync(Note note);
        Task DeleteNoteAsync(int id);
        IQueryable<Note> GetNotesQueryable();
        Task<string> ExportToCsvAsync();
    }
}

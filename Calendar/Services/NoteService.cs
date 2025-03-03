using Calendar.Models;
using Calendar.Repositories.Interfaces;
using Calendar.Services.Interfaces;
using System.Text;

namespace Calendar.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync() => await _noteRepository.GetAllAsync();

        public async Task<Note> GetNoteByIdAsync(int id) => await _noteRepository.GetByIdAsync(id);

        public async Task CreateNoteAsync(Note note) => await _noteRepository.AddAsync(note);

        public async Task UpdateNoteAsync(Note note) => await _noteRepository.UpdateAsync(note);

        public async Task DeleteNoteAsync(int id) => await _noteRepository.DeleteAsync(id);

        public IQueryable<Note> GetNotesQueryable() => _noteRepository.GetQueryable();
        public async Task<string> ExportToCsvAsync()
        {
            var notes = await _noteRepository.GetAllAsync();
            var csv = new StringBuilder();
            csv.AppendLine("Id,Title,Content,CreatedAt,ReminderTime"); // Заголовки CSV

            foreach (var note in notes)
            {
                csv.AppendLine($"{note.Id},{note.Title},{note.Content},{note.CreatedAt},{note.ReminderTime}");
            }

            return csv.ToString();
        }
    }
}


using Calendar.Models;
using Calendar.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Calendar.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;

        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllAsync() => await _context.Notes.ToListAsync();

        public async Task<Note> GetByIdAsync(int id) => await _context.Notes.FindAsync(id);

        public async Task AddAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Note> GetQueryable() => _context.Notes.AsQueryable();
    }
}

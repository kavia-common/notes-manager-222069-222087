using System.Collections.Concurrent;
using NotesManagerBackend.Models;

namespace NotesManagerBackend.Repositories
{
    /// <summary>
    /// Thread-safe in-memory notes repository.
    /// </summary>
    public class InMemoryNotesRepository : INotesRepository
    {
        private readonly ConcurrentDictionary<Guid, Note> _store = new();

        public IEnumerable<Note> GetAll()
        {
            return _store.Values.OrderByDescending(n => n.UpdatedAt);
        }

        public Note? GetById(Guid id)
        {
            _store.TryGetValue(id, out var note);
            return note;
        }

        public void Add(Note note)
        {
            _store[note.Id] = note;
        }

        public bool Update(Note note)
        {
            if (!_store.ContainsKey(note.Id))
            {
                return false;
            }
            _store[note.Id] = note;
            return true;
        }

        public bool Delete(Guid id)
        {
            return _store.TryRemove(id, out _);
        }
    }
}

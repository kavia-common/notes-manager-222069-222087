using NotesManagerBackend.DTOs;
using NotesManagerBackend.Models;
using NotesManagerBackend.Repositories;

namespace NotesManagerBackend.Services
{
    /// <summary>
    /// Default notes service implementing business rules.
    /// </summary>
    public class NotesService : INotesService
    {
        private readonly INotesRepository _repository;

        public NotesService(INotesRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Note> GetAll() => _repository.GetAll();

        public Note? Get(Guid id) => _repository.GetById(id);

        public Note Create(NoteCreateDto dto)
        {
            var now = DateTime.UtcNow;
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = dto.Title.Trim(),
                Content = string.IsNullOrEmpty(dto.Content) ? null : dto.Content,
                CreatedAt = now,
                UpdatedAt = now
            };
            _repository.Add(note);
            return note;
        }

        public Note? Update(Guid id, NoteUpdateDto dto)
        {
            var existing = _repository.GetById(id);
            if (existing == null) return null;

            existing.Title = dto.Title.Trim();
            existing.Content = string.IsNullOrEmpty(dto.Content) ? null : dto.Content;
            existing.UpdatedAt = DateTime.UtcNow;

            var ok = _repository.Update(existing);
            return ok ? existing : null;
        }

        public bool Delete(Guid id) => _repository.Delete(id);
    }
}

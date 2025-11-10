using NotesManagerBackend.DTOs;
using NotesManagerBackend.Models;

namespace NotesManagerBackend.Services
{
    /// <summary>
    /// Business logic layer for notes.
    /// </summary>
    public interface INotesService
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieve all notes.
        /// </summary>
        IEnumerable<Note> GetAll();

        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieve a single note by identifier.
        /// </summary>
        Note? Get(Guid id);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Create a new note from the provided DTO.
        /// </summary>
        Note Create(NoteCreateDto dto);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Update an existing note.
        /// </summary>
        /// <returns>The updated note if successful; otherwise null.</returns>
        Note? Update(Guid id, NoteUpdateDto dto);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Delete a note by ID.
        /// </summary>
        bool Delete(Guid id);
    }
}

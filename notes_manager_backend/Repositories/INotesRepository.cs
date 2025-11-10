using NotesManagerBackend.Models;

namespace NotesManagerBackend.Repositories
{
    /// <summary>
    /// Repository abstraction for managing notes.
    /// </summary>
    public interface INotesRepository
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Get all notes.
        /// </summary>
        /// <returns>Enumerable of notes.</returns>
        IEnumerable<Note> GetAll();

        // PUBLIC_INTERFACE
        /// <summary>
        /// Get a note by ID.
        /// </summary>
        /// <param name="id">Note ID.</param>
        /// <returns>The note if found; otherwise null.</returns>
        Note? GetById(Guid id);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Create and store a new note.
        /// </summary>
        /// <param name="note">Note to add.</param>
        void Add(Note note);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Update an existing note.
        /// </summary>
        /// <param name="note">Note with updated fields.</param>
        /// <returns>True if updated, false otherwise.</returns>
        bool Update(Note note);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Delete a note by ID.
        /// </summary>
        /// <param name="id">Note ID.</param>
        /// <returns>True if deleted, false otherwise.</returns>
        bool Delete(Guid id);
    }
}

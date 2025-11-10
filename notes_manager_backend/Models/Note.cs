using System;

namespace NotesManagerBackend.Models
{
    /// <summary>
    /// Domain model representing a Note entity.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Unique identifier of the note.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the note. Required.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Content of the note. Optional.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Created timestamp in UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Updated timestamp in UTC.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}

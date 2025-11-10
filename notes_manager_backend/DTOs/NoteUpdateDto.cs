using System.ComponentModel.DataAnnotations;

namespace NotesManagerBackend.DTOs
{
    /// <summary>
    /// DTO for updating a note.
    /// </summary>
    public class NoteUpdateDto
    {
        /// <summary>
        /// Title of the note.
        /// </summary>
        [Required]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 120 characters.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Content of the note (optional).
        /// </summary>
        [MaxLength(10000, ErrorMessage = "Content cannot exceed 10000 characters.")]
        public string? Content { get; set; }
    }
}

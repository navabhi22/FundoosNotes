using DatabaseLayer.Notes;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Interfaces
{
    public interface INoteBL
    {
        Task AddNote(NotesPostModel notesPostModel, int UserID);
        Task<Note> UpdateNote(int userId, int noteId, NoteUpdateModel noteUpdateModel);
        Task DeleteNote(int userId, int noteId);
        Task ChangeColour(int userId, int noteId, string color);

        Task ArchiveNote(int userId, int noteId);
    }
}

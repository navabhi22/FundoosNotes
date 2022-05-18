using BuisnessLayer.Interfaces;
using DatabaseLayer.Notes;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Services
{
    public class NoteBL : INoteBL
    {
        INoteRL noteRL;
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }

        public async Task AddNote(NotesPostModel notesPostModel, int UserID)
        {

            try
            {
                await this.noteRL.AddNote(notesPostModel, UserID);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
    }
}


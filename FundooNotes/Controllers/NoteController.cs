using BuisnessLayer.Interfaces;
using DatabaseLayer.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.DBContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    public class NoteController : ControllerBase
    {
        FundooContext fundooContext;
        INoteBL noteBL;

        // constructor
        public NoteController(FundooContext fundoo, INoteBL noteBL)
        {
            this.fundooContext = fundoo;
            this.noteBL = noteBL;
        }

        /// <summary>
        /// Add Notes
        /// </summary>
        /// <param name="notesPostModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddNote")]
        public async Task<ActionResult> AddUser(NotesPostModel notesPostModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                await this.noteBL.AddNote(notesPostModel, userId);

                return this.Ok(new { success = true, message = $"Note Added Successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

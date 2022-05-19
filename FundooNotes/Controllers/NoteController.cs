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
    [ApiController]
    [Route("[controller]")]
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

                return this.Ok(new { success = true, message = $"Notes Added Successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Update Notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="noteUpdateModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Update/{noteId}")]
        public async Task<ActionResult> UpdateNote(int noteId, NoteUpdateModel noteUpdateModel)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == UserId && u.NoteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to Update note" });
                }
                await this.noteBL.UpdateNote(UserId, noteId, noteUpdateModel);
                return this.Ok(new { success = true, message = "Note Updated successfully!!!" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Delete the Notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{noteId}")]
        public async Task<ActionResult> DeleteNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == UserId && u.NoteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Oops! This note is not available " });

                }
                await this.noteBL.DeleteNote(noteId, UserId);
                return this.Ok(new { success = true, message = "Note Deleted Successfully" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Change colour of the Notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="colour"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ChangeColour/{noteId}/{colour}")]
        public async Task<ActionResult> ChangeColour(int noteId, string colour)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == UserId && u.NoteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Sorry! Note does not exist" });
                }

                await this.noteBL.ChangeColour(UserId, noteId, colour);
                return this.Ok(new { success = true, message = "Note Colour Changed Successfully " });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Archive Notes
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ArchiveNote/{noteId}")]
        public async Task<ActionResult> IsArchieveNote(int noteId)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userID", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.Userid == userId && u.NoteID == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Failed to archieve notes or Id does not exists" });
                }
                await this.noteBL.ArchiveNote(userId, noteId);
                return this.Ok(new { success = true, message = "Note Archieved successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

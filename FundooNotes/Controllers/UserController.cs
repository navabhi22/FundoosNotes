using BuisnessLayer.Interfaces;
using DatabaseLayer;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.DBContext;
using RepositoryLayer.Interfaces;
using System;

namespace FundooNotes.Controllers
{
    public class UserController : ControllerBase
    {
        IUserBL userBL;
        FundooContext fundooContext;
        public UserController(IUserBL userBL, FundooContext fundooContext)
        {
            this.userBL = userBL;
            this.fundooContext = fundooContext;
        }
        [HttpPost("AddUser")]
        public ActionResult AddUser(UserPostModel user)
        {
            try
            {
                this.userBL.AddUser(user);
                return this.Ok(new {success=true, message="user Added Successfully"});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

using BuisnessLayer.Interfaces;
using DatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.DBContext;
using RepositoryLayer.Services;
using RepositoryLayer.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
                return this.Ok(new { success = true, message = $"User Added Successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("{Email}/{Password}")]
        public ActionResult LoginUser(string Email, string Password)
        {
            try
            {
                var userdata = fundooContext.users.FirstOrDefault(u => u.Email == Email);
                string password = StringCipher.DecodeFrom64(userdata.Password);
                if (userdata == null)
                {
                    return this.BadRequest(new { success = false, message = $"email and password is invalid" });

                }

                string result = this.userBL.LoginUser(Email, Password);
                return this.Ok(new { success = true, message = $"login successfull Token {result}",  });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("ForgotPassword/{email}")]
        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var Result = this.userBL.ForgetPassword(email);
                if (Result != false)
                {
                    return this.Ok(new

                    {
                        success = true,
                        message = $"mail sent sucessfully" + $"token: {Result}"
                    });
                }

                return this.BadRequest(new { success = false, message = $"mail not sent" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Authorize]
        [HttpPut("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordModel changePassword)
        {
            try
            {
                var UserId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UserId", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(UserId.Value);
                var result = fundooContext.users.Where(u => u.UserId == UserID).FirstOrDefault();
                string Email = result.Email.ToString();

                bool res = userBL.ChangePassword(Email, changePassword);//email.changepass
                if (res == false)
                {
                    return this.BadRequest(new { success = false, message = "Enter Valid Password" });
                }
                return this.Ok(new { success = true, message = "Password changed Successfully" });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}

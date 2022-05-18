using BuisnessLayer.Interfaces;
using DatabaseLayer;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    public class UserBL : IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public void AddUser(UserPostModel user)
        {
            try
            {
                this.userRL.AddUser(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string LoginUser(string Email, string Password)
        {
            try
            {
                return this.userRL.LoginUser(Email, Password);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ChangePassword(string Email, ChangePasswordModel changePassword)
        {
            try
            {
                return this.userRL.ChangePassword(Email, changePassword);

            }
            catch (Exception)
            {

                throw;
            }
        }

        bool IUserBL.ForgetPassword(string Email)
        {
            try
            {
                return this.userRL.ForgetPassword(Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

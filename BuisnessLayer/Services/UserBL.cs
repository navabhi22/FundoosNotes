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
                catch (Exception ex)
                {
                    throw ex;
                }
            
        }
    }
}

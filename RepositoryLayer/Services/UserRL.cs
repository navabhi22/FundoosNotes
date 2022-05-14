using DatabaseLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.DBContext;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        FundooContext fundooContext;
        public IConfiguration Configuration { get; }
        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.Configuration = configuration;
        }
        

        public void AddUser(UserPostModel user)
        {
            try
            {
                User userdata = new User();
                userdata.FirstName = user.FirstName;
                userdata.LastName = user.LastName;
                userdata.Email = user.Email;
                userdata.Password = user.Password;
                userdata.DateCreated= DateTime.Now;
                userdata.Address = user.Address;
                fundooContext.users.Add(userdata);
                fundooContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

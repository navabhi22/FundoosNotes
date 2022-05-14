using DatabaseLayer;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interfaces
{
    public interface IUserBL
    {
        public void AddUser(UserPostModel user);
    }
}

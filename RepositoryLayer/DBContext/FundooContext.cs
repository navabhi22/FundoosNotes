using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.DBContext
{
    public class FundooContext:DbContext
    {
        public FundooContext(DbContextOptions options)
           : base(options)
        {

        }

        public DbSet<User> users { get; set; }
    }
}

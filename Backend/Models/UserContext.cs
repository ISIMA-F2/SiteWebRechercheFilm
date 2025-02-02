using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TrackerDeFavorisApi.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options){

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options){
            options.UseSqlite("Data Source=User.db");
        }
        public DbSet<User> Users { get; set;} = null!;
    }
}
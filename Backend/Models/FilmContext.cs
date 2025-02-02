using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TrackerDeFavorisApi.Models
{
    public class FilmContext : DbContext
    {
        public FilmContext(DbContextOptions<FilmContext> options) : base(options){

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options){
            options.UseSqlite("Data Source=Film.db");
        }
        public DbSet<Film> Films { get; set;} = null!;
    }
}
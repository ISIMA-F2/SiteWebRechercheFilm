using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TrackerDeFavorisApi.Models
{
    public class FavorisContext : DbContext
    {
        public FavorisContext(DbContextOptions<FavorisContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=Favoris.db");
        }
        public DbSet<Favoris> Favoriss { get; set; } = null!;
    }
}
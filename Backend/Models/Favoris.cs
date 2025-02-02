using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace TrackerDeFavorisApi.Models
{
    public class Favoris
    {
        public int Id { get; set; } = 0;
        public string UserId { get; set; } = "";
        public string Film { get; set; } = "";
        public string Poster { get; set; } = "";
    }
}
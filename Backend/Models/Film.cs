using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace TrackerDeFavorisApi.Models
{
    public class Film
    {
        public int Id{get; set;} = 0;
        public string Title{get; set;} = "";
        public string Poster{get; set;} = "";
        public string Imdb{get; set;} = "";
        public int Year{get; set;} = 0;
    }
}
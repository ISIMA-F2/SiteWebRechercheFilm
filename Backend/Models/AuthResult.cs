using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerDeFavorisApi.Models;

namespace Backend.Models
{
    public class AuthResult
    {        
        public string? Token { get; set; }
        public User? User { get; set; }
        public string? Message { get; set; }
    }
}
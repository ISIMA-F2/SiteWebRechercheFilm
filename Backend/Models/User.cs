using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerDeFavorisApi.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = "";
        public string Nom{ get; set; } = string.Empty;
        public string Email{ get; set; } = "";
        public string Prenom{ get; set; } = "";
        public string MotDePasse{ get; set; } = "";
        public User.Role RoleType { get; set; } = User.Role.User;
        public enum Role{
            Admin,
            User
        }
    }
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
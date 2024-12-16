using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities
{
    internal class User
    {

        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string HashPassword { get; set; }
        public string Email { get; set; }
        //Role 
        // AssinedAdmin

    }
}

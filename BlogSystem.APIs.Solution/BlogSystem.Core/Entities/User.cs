using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities
{
    public class User : BaseClassWithId
    {
       
        [Required]
        public string UserName { get; set; }
        public string HashPassword { get; set; }
        public string Email { get; set; }
        UserRole Role { get; set; } = UserRole.Reader;
    }
}

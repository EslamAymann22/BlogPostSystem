using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {

        [Required]
        public string DisplayName { get; set; }
        public UserRole Role { get; set; } = UserRole.Reader;

    }
}

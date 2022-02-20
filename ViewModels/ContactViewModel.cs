using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.ViewModels
{
    public class ContactViewModel
    {
        [Required ]
        [MinLength(5 , ErrorMessage ="too short")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}

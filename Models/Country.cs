using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLEXAPI.Models
{
    public class Country
    {

        public int Id { get; set; }

        [StringLength(64)]
        [MaxLength(64)]
        public string Guid { get; set; }

        [Required(ErrorMessage = "Please enter a Name")]
        [StringLength(255)]
        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; }

        [StringLength(32)]
        [MaxLength(32)]
        public string Code { get; set; }


    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UsersLivetrackerConfigDAL.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        [MaxLength(5000)]
        public string TokenHash { get; set; }

        public List<Keyword> Keywords { get; set; }
    }
}

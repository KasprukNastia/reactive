using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UsersLivetrackerConfigDAL.Models
{
    public class Keyword
    {
        public int KeywordId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Word { get; set; }

        public List<User> Users { get; set; }
    }
}

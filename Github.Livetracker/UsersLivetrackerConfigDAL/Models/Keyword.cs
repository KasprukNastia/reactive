using System.ComponentModel.DataAnnotations;

namespace UsersLivetrackerConfigDAL.Models
{
    public class Keyword
    {
        public int KeywordId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Word { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}

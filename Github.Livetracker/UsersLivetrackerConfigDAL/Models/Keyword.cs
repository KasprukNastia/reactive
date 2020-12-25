using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UsersLivetrackerConfigDAL.Models
{
    public class Keyword
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Word { get; set; }

        public List<User> Users { get; set; }

        public List<KeywordInfo> KeywordInfos { get; set; }
    }
}

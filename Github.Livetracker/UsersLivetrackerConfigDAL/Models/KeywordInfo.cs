using System.ComponentModel.DataAnnotations;

namespace UsersLivetrackerConfigDAL.Models
{
    public class KeywordInfo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Word { get; set; }

        [Required]
        [MaxLength(500)]
        public string Source { get; set; }

        [Required]
        [MaxLength(500)]
        public string FileName { get; set; }

        [MaxLength(500)]
        public string RelativePath { get; set; }

        [Required]
        [MaxLength(1000)]
        public string FileUrl { get; set; }

        [Required]
        [MaxLength(1000)]
        public string RepositoryUrl { get; set; }

        [Required]
        public int KeywordId { get; set; }

        public Keyword Keyword { get; set; }
    }
}

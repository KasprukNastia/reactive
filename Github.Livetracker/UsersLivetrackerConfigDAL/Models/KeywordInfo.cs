using System.ComponentModel.DataAnnotations;

namespace UsersLivetrackerConfigDAL.Models
{
    public class KeywordInfo
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string ShaHash { get; set; }

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

        public int? KeywordId { get; set; }

        public bool? WasProcessed { get; set; }

        public Keyword Keyword { get; set; }
    }
}

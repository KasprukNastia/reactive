namespace SettingsProxyAPI.Models
{
    public class KeywordInfo
    {
        public string Keyword { get; set; }
        public string FileName { get; set; }
        public string RelativePath { get; set; }
        public  string FileUrl { get; set; }
        public string RepositoryUrl { get; set; }
        public int? TotalCount { get; set; }
    }
}

namespace LemonPlatform.Core.Databases.Models
{
    public class UserPreference
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ModuleName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

    }
}
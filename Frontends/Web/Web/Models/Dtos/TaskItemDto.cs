namespace TaskManagementApp.Frontends.Web.Models.Dtos
{
    public class TaskItemDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        [JsonPropertyName("name")]
        [Required(ErrorMessage = "Task's name cannot be empty!")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        [StringLength(50, ErrorMessage = "Description cannot be more than 50 characters")]
        public string? Description { get; set; }
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        [JsonPropertyName("createTime")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        [JsonPropertyName("modifiedAt")]
        public DateTime? ModifiedAt { get; set; }
    }
}

namespace TaskManagementApp.Services.TasksApi.Models
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
    }
}

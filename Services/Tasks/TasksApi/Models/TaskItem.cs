namespace TaskManagementApp.Services.TasksApi.Models
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MinLength(3), MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}

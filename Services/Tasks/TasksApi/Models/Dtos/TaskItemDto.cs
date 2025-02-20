namespace TaskManagementApp.Services.TasksApi.Models.Dtos
{
    public class TaskItemDto : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        [StringLength(50)]
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
    }
}

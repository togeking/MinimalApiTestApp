using api.Repositories.Enums;

namespace api.Repositories.Entities;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int UserId { get; set;}
    public int AssigneeId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public TodoStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
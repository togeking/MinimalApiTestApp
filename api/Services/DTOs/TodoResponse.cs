namespace api.Services.DTOs;

public record TodoResponse(int Id, string Title, int UserId, int AssigneeId, DateTime? StartDate, DateTime? EndDate, string Status, DateTime CreatedAt);
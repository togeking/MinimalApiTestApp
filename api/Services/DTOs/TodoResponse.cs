namespace api.Services.DTOs;

public record TodoResponse(int Id, string Title, int UserId, int AssigneeId, DateOnly? StartDate, DateOnly? EndDate, string Status, DateTime CreatedAt);
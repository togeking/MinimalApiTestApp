namespace api.Services.DTOs;

public record TodoCreateRequest(string Title, int UserId, int AssigneeId, DateTime? StartDate, DateTime? EndDate, string Status);
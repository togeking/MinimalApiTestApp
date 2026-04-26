namespace api.Services.DTOs;

public record TodoCreateRequest(string Title, int UserId, int AssigneeId, DateOnly? StartDate, DateOnly? EndDate, string Status);
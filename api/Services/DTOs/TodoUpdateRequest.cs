namespace api.Services.DTOs;

public record TodoUpdateRequest(int Id, string Title, int UserId, int AssigneeId, DateOnly? StartDate, DateOnly? EndDate, string Status);
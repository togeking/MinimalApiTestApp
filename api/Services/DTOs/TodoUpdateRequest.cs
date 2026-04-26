namespace api.Services.DTOs;

public record TodoUpdateRequest(int Id, string Title, int UserId, int AssigneeId, DateTime? StartDate, DateTime? EndDate, string Status);
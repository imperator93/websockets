namespace Api.Dto;

public record UserRequest(
    string Name,
    string Password,
    string Avatar
);

public record UserResponse(
    Guid Id,
    string Name,
    string Avatar,
    bool IsOnline
);
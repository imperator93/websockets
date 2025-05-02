namespace Api.Dto;

public record UserRequest(
    string Name,
    string Password,
    bool IsOnline,
    string Avatar
);

public record UserResponse(
    Guid Id,
    string Name,
    string Avatar,
    bool IsOnline
);
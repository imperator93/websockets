namespace Api.Dto;

public record UserRegisterRequest(
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

public record UserLoginRequest(
    string Name,
    string Password
);
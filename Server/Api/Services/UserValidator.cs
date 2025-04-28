using Api.Data;
using Api.Dto;
using Api.Repository;
using FluentValidation;

namespace Api.Services;

public class UserRegisterValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterValidator(IUserRepository userRepository)
    {
        RuleFor(u => u.Name).Cascade(CascadeMode.Stop).MustAsync(async (name, _) =>
        {
            return await userRepository.UserExists(name);
        }).WithErrorCode("UsernameTaken").WithMessage("User exists! Pick another name!").NotEmpty().WithErrorCode("UsernameEmpty").WithMessage("{PropertyName} must not be empty").Length(3, 15).WithErrorCode("InvalidLength").WithMessage("{PropertyName} must be at least 3 and at most 15 characters long");

        RuleFor(u => u.Password).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("{PropertyName} must not be empty").Length(3, 15).WithMessage("{PropertyName} must be at least 3 and at most 15 characters long");
    }
}

public class UserLoginValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginValidator(IUserRepository userRepository, EncryptionService encryptionService)
    {
        RuleFor(u => u.Name).MustAsync(async (name, _) =>
        {
            var user = await userRepository.GetUserByName(name);
            return user is not null;

        }).WithErrorCode("UserIsNull").WithMessage("User doesn't exit");

        RuleFor(u => u.Password).MustAsync(async (u, password, _) =>
        {
            var user = await userRepository.GetUserByName(u.Name);
            return encryptionService.Decrypt(user!.Password) == password;

        }).WithMessage("Incorrect Password");
    }
}
using Api.Dto;
using Api.Models;
using FluentValidation;

namespace Api.Services;

public class UserRegisterValidator : AbstractValidator<UserRequest>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x).Custom((request, context) =>
        {
            var user = context.RootContextData["User"] as User;

            if (user is not null)
            {
                context.AddFailure("User", "User already exists");
                return;
            }
        });

        RuleFor(x => x.Password).Cascade(CascadeMode.Stop).Matches("^[a-zA-Z0-9]+$").WithMessage("Password must not contain special characters like \' or \"");
    }
}

public class UserLoginValidator : AbstractValidator<UserRequest>
{
    public UserLoginValidator()
    {
        RuleFor(x => x).Custom((request, context) =>
               {
                   var user = context.RootContextData["User"] as User;

                   if (user is null)
                   {
                       context.AddFailure("User", "User doesn't exist!");
                       return;
                   }

                   if (request.Password != user.Password)
                   {
                       context.AddFailure("Password", "Incorrect password!");
                       return;
                   }
               });
    }
}
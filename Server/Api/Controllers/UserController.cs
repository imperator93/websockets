using Api.Dto;
using Api.Repository;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{

    private readonly IUserRepository _userRepository;
    private readonly UserRegisterValidator _userRegisterValidator;
    private readonly UserLoginValidator _userLoginValidator;

    public UserController(IUserRepository userRepository, UserRegisterValidator userRegisterValidator, UserLoginValidator userLoginValidator)
    {
        _userRepository = userRepository;
        _userRegisterValidator = userRegisterValidator;
        _userLoginValidator = userLoginValidator;
    }

    [HttpGet("/users")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userRepository.GetAllUsers());
    }

    [HttpPost("/user/register")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register(UserRequest userRequest)
    {
        List<UserError> errors = [];

        var results = await _userRegisterValidator.ValidateAsync(userRequest);

        if (!results.IsValid)
        {
            foreach (var error in results.Errors)
            {
                errors.Add(new UserError(error.ErrorCode, error.ErrorMessage));
            }

            foreach (var i in errors) System.Console.WriteLine(i);

            return BadRequest(errors);
        }

        var userResponse = await _userRepository.CreateUser(userRequest);

        return Ok(userResponse);
    }

    [HttpPost("/user/login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login(UserRequest userRequest)
    {
        var results = _userLoginValidator.Validate(userRequest);

        List<UserError> errors = [];

        if (!results.IsValid)
        {
            foreach (var error in results.Errors)
            {
                errors.Add(new UserError(error.ErrorCode, error.ErrorMessage));
            }
            return BadRequest(errors);
        }

        var userResponse = await _userRepository.LoginUser(userRequest.Name);

        return Ok(userResponse);
    }

    public record UserError(
        string ErrorCode,
        string ErrorMessage
    );
}
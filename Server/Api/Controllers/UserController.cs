using Api.Dto;
using Api.Repository;
using Api.Services;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{

    private readonly IUserRepository _userRepository;
    private readonly UserRegisterValidator _userRegisterValidator;
    private readonly UserLoginValidator _userLoginValidator;
    public readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, UserRegisterValidator userRegisterValidator, UserLoginValidator userLoginValidator, IMapper mapper)
    {
        _userRepository = userRepository;
        _userRegisterValidator = userRegisterValidator;
        _userLoginValidator = userLoginValidator;
        _mapper = mapper;
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
    public async Task<IActionResult> Register(UserRequest userRegisterRequest)
    {
        List<UserError> errors = [];

        var user = await _userRepository.GetUserByName(userRegisterRequest.Name);

        var context = new ValidationContext<UserRequest>(userRegisterRequest);
        context.RootContextData["User"] = user;

        var results = _userRegisterValidator.Validate(context);

        if (!results.IsValid)
        {
            foreach (var error in results.Errors)
            {
                errors.Add(new UserError(error.ErrorCode, error.ErrorMessage));
            }
            return BadRequest(errors);
        }

        var userResponse = await _userRepository.CreateUser(userRegisterRequest);

        return Ok(userResponse);
    }

    [HttpPost("/user/login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login(UserRequest userLoginRequest)
    {
        List<UserError> errors = [];

        var user = await _userRepository.GetUserByName(userLoginRequest.Name);
        var context = new ValidationContext<UserRequest>(userLoginRequest);
        context.RootContextData["User"] = user;

        var results = _userLoginValidator.Validate(context);

        if (!results.IsValid)
        {
            foreach (var error in results.Errors)
            {
                errors.Add(new UserError(error.ErrorCode, error.ErrorMessage));
            }
            return BadRequest(errors);
        }

        var userResponse = _mapper.Map<UserResponse>(user);

        return Ok(userResponse);
    }

    [HttpPut("/user")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PutUser(UserRequest userRequest)
    {
        bool success = await _userRepository.ChangeUserAndSaveToDb(userRequest);

        return success ? Ok("User updated!") : BadRequest(new UserError(ErrorCode: "NotFound",
        ErrorMessage: "User not found"));
    }

    public record UserError(
        string ErrorCode,
        string ErrorMessage
    );
}
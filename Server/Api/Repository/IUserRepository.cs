using Api.Dto;
using Api.Models;

namespace Api.Repository;

public interface IUserRepository
{
    Task<List<UserResponse>> GetAllUsers();
    Task<bool> UserExists(string name);
    Task<User> AddUserToDb(UserRequest userRequest);
    Task<User?> GetUserByName(string name);
    Task<UserResponse> CreateUser(UserRequest userRequest);
    Task<bool> ChangeUserAndSaveToDb(UserRequest userRequest);
}
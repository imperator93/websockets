using Api.Data;
using Api.Dto;
using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace Api.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    private readonly EncryptionService _encryptionService;
    private readonly IMapper _mapper;
    public UserRepository(DataContext dataContext, IMapper mapper, EncryptionService encryptionService)
    {
        _dataContext = dataContext;
        _encryptionService = encryptionService;
        _mapper = mapper;
    }

    public async Task<List<UserResponse>> GetAllUsers()
    {
        var users = await _dataContext.Users.ToArrayAsync();

        List<UserResponse> userReponses = [];

        foreach (var user in users)
        {
            userReponses.Add(_mapper.Map<UserResponse>(user));
        }
        return userReponses;
    }

    public async Task<bool> UserExists(string name)
    {
        return !await _dataContext.Users.AnyAsync(u => u.Name == name);
    }

    public async Task<User?> GetUserByName(string name)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Name == name);
        if (user is not null)
        {
            user.Password = _encryptionService.Decrypt(user.Password);
            return user;
        }
        return user;
    }

    public async Task<User> AddUserToDb(UserRegisterRequest userRequest)
    {
        var user = _mapper.Map<User>(userRequest);

        user.Password = _encryptionService.Encrypt(user.Password);

        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();

        return user;
    }
    public async Task<UserResponse> CreateUser(UserRegisterRequest userRequest)
    {
        var user = await AddUserToDb(userRequest);

        return _mapper.Map<UserResponse>(user);
    }

}


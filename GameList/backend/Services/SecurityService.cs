using AutoMapper;
using BCrypt.Net;

public class SecurityService {
    private readonly IGameListRepository _repository;
    private readonly IMapper _mapper;

    public string EncryptPasswordAsync(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool ValidatePasswordAsync(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    Task<string> CreateAccessTokenAsync(Guid listId) {

    }
    Task<bool> ValidateAccessTokenAsync(string token);
}
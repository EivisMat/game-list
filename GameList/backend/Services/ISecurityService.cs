public interface ISecurityService {
    string EncryptPasswordAsync(string password);
    bool ValidatePasswordAsync(string password);

    Task<string> CreateAccessTokenAsync(Guid listId);
    Task<bool> ValidateAccessTokenAsync(string token);
}
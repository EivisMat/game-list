public interface ISecurityService {
    string EncryptPassword(string password);
    bool ValidatePassword(string password, string hashedPassword);

    string CreateAccessToken(Guid listId);
    bool ValidateAccessToken(string token);
}
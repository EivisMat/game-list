public interface ISecurityService {
    string HashPassword(string password);
    bool ValidatePassword(string password, string hashedPassword);

    string CreateAccessToken(Guid listId);
    AuthValidationResult ValidateHttpRequest(HttpRequest request);
}
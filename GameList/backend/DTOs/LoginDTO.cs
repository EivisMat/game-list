namespace GameList.DTOs;

public class LoginDTO
{
    public string Name { get; set; }
    public string Password { get; set; }

    public LoginDTO(){
        Name = string.Empty;
        Password = string.Empty;
    }

    public LoginDTO(string Name, string Password) {
        this.Name = Name;
        this.Password = Password;
    }
}

namespace GameList.DTOs;

public class GameListDTO
{
    public string Name { get; set; }
    public string Password { get; set; }
    public List<string> Games { get; set; }
    public List<string> People { get; set; }

    public GameListDTO(string Name, string Password, List<string> Games, List<string> People) {
        if (string.IsNullOrEmpty(Name)) {
            throw new ArgumentException("Name cannot be null or empty", nameof(Name));
        }

        if (string.IsNullOrEmpty(Password)) {
            throw new ArgumentException("Password cannot be null or empty", nameof(Password));
        }

        this.Name = Name;
        this.Password = Password;
        this.Games = Games;
        this.People = People;
    }
}
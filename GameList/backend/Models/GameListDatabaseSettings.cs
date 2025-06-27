namespace GameList.Models;

public class GameListDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string GamesCollectionName { get; set; } = null!;
}
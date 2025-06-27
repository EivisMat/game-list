namespace GameList.DTOs;

public class UpdateGameListDTO
{
    public string Action { get; set; }
    public string Target { get; set; }
    public string Value { get; set; }

    public UpdateGameListDTO() {
        Action = string.Empty;
        Target = string.Empty;
        Value = string.Empty;
    }

    public UpdateGameListDTO(string Action, string Target, string Value) {
        this.Action = Action;
        this.Target = Target;
        this.Value = Value;
    }
}
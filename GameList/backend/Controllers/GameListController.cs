using GameList.Models;
using GameList.Services;
using GameList.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace GameList.Controllers;

[ApiController]
[Route("api/lists")]
public class GamesController : ControllerBase {
    private readonly GameListsService _gamesService;
    private readonly SecurityService _securityService;

    public GamesController(GameListsService gamesService, SecurityService securityService) {
        _gamesService = gamesService;
        _securityService = securityService;
    }

    // POST /api/lists/create
    // Creates a new game list
    // Request body: { 
    //  "name" : "string", 
    //  "password" : "string", 
    //  "games" : ["string"], 
    //  "people" : ["string"] 
    // }
    // => Returns: {"id": "string", "token" : "string"} // JWT token for auth
    [HttpPost("create")]
    public async Task<IActionResult> CreateGameList([FromBody] GameListDTO newListDTO) {
        if (newListDTO == null || string.IsNullOrWhiteSpace(newListDTO.Name)) {
            return BadRequest(new { message = "Invalid game list data." });
        }

        // Check if a list with the same name already exists
        var existingList = await _gamesService.GetByNameAsync(newListDTO.Name);
        if (existingList != null) { 
            return Conflict(new { message = "A game list with this name already exists." });
        }
        newListDTO.Password = _securityService.HashPassword(newListDTO.Password);

        Models.GameList createdList = new Models.GameList {
            Name = newListDTO.Name,
            Password = newListDTO.Password,
            Games = newListDTO.Games.Select(g => new Game { Name = g }).ToList(),
            People = newListDTO.People.Select(p => new Person { Name = p }).ToList()
        };

        // Populate Owners list for each game
        foreach (Game game in createdList.Games) {
            foreach (Person person in createdList.People) {
                game.AddOwner(person.Id, false); // default to not owning
            }
        }

        await _gamesService.CreateAsync(createdList);

        // Generate JWT token for authentication
        string token = _securityService.GenerateJwtToken(createdList.Id!);

        // Return created list ID and token
        return CreatedAtAction(
            nameof(GetListById),
            new { id = createdList.Id },
            new { id = createdList.Id, token = token }
        );

    }

    // POST /api/lists/login
    // Accesses a game list by name and password
    // Request body: { "name": "string", "password": "string" }
    // => Returns: {"id": "string", "token" : "string"} //
    [HttpPost("login")]
    public async Task<IActionResult> LoginToGameList([FromBody] LoginDTO loginDTO) {
        if (loginDTO == null || string.IsNullOrWhiteSpace(loginDTO.Name) || string.IsNullOrWhiteSpace(loginDTO.Password)) {
            return BadRequest(new { message = "Invalid login data." });
        }

        Models.GameList gameList = await _gamesService.GetByNameAsync(loginDTO.Name);

        if (gameList == null) {
            return NotFound(new { message = "Game list not found." });
        }

        if (!_securityService.VerifyPassword(gameList.Password, loginDTO.Password)) {
            return Unauthorized(new { message = "Incorrect password." });
        }

        // Generate JWT token for authentication
        var token = _securityService.GenerateJwtToken(gameList.Id!);

        // Return game list ID and token
        return Ok(new { id = gameList.Id, token });
    }

    // GET /api/lists/{id}
    // HEADER: Authorization: Bearer {token}
    // Retrieves a game list by its ID
    // => Returns: GameList object without password
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.GameList>> GetListById(string id) {
        // Validate the JWT token
        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        string? listId = _securityService.ValidateJwtToken(token);

        if (listId == null || listId != id) {
            return Unauthorized(new { message = "Invalid token." });
        }

        Models.GameList gameList = await _gamesService.GetByIdAsync(id);

        if (gameList == null) {
            return NotFound(new { message = "Game list not found." });
        }

        // Return the game list without the password
        return Ok(new {
            gameList.Id,
            gameList.Name,
            gameList.Games,
            gameList.People,
            gameList.RandomlyPickedGame
        });
    }

    // PATCH /api/lists/{id}
    // HEADER: Authorization: Bearer {token}
    // BODY: {"action": "string", "target": "string", "value": "string"}
    // "action": "add"/"remove", "target": "game"/"person", "value": "string" adds or removes person/game from DB. if remove, value not used.
    // "action": "toggleowner", "target": "{personid}", "value": "{gameId}" toggles personId ownership of gameId
    // "action": "toggleexclude", "target": "game", "value": "not used" toggles exclusion of game from being picked randomly
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateGameList(string id, [FromBody] UpdateGameListDTO updateDTO) {
        // Validate the JWT token
        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        string? listId = _securityService.ValidateJwtToken(token);

        if (listId == null || listId != id) {
            return Unauthorized(new { message = "Invalid token." });
        }

        Models.GameList gameList = await _gamesService.GetByIdAsync(id);
        if (gameList == null) {
            return NotFound(new { message = "Game list not found." });
        }

        switch (updateDTO.Action?.ToLower()) {
            case "add":
                if (updateDTO.Target?.ToLower() == "game" && !string.IsNullOrWhiteSpace(updateDTO.Value)) {
                    if (!gameList.Games.Any(g => g.Name == updateDTO.Value)) {
                        Game newGame = new Game { Name = updateDTO.Value };
                        // Add owners for each person
                        foreach (Person person in gameList.People) {
                            newGame.AddOwner(person.Id, false);
                        }
                        gameList.Games.Add(newGame);
                    }
                }
                else if (updateDTO.Target?.ToLower() == "person" && !string.IsNullOrWhiteSpace(updateDTO.Value)){
                    if (!gameList.People.Any(p => p.Name == updateDTO.Value)) {
                        Person newPerson = new Person { Name = updateDTO.Value };
                        gameList.People.Add(newPerson);

                        // Add owner entry for each game
                        foreach (Game game in gameList.Games) {
                            game.AddOwner(newPerson.Id, false);
                        }
                    }
                }
                break;

            case "remove":
                if (updateDTO.Target?.ToLower() == "game" && !string.IsNullOrWhiteSpace(updateDTO.Value)) {
                    Game? gameToRemove = gameList.Games.FirstOrDefault(g => g.Id == updateDTO.Value);
                    if (gameToRemove != null) {
                        gameList.Games.Remove(gameToRemove);
                    }
                    else {
                        return NotFound(new { message = "Game not found." });
                    }
                }
                else if (updateDTO.Target?.ToLower() == "person" && !string.IsNullOrWhiteSpace(updateDTO.Value)) {
                    Person? personToRemove = gameList.People.FirstOrDefault(p => p.Id == updateDTO.Value);
                    if (personToRemove != null) {
                        gameList.People.Remove(personToRemove);
                        
                        // Remove owner entry from each game
                        foreach (Game game in gameList.Games) {
                            game.RemoveOwner(personToRemove.Id);
                        }
                    }
                    else {
                        return NotFound(new { message = "Person not found." });
                    }
                }
                break;

            case "toggleowner":
                if (!string.IsNullOrWhiteSpace(updateDTO.Target) && !string.IsNullOrWhiteSpace(updateDTO.Value)) {
                    string[] parts = updateDTO.Target.Split('|');
                    if (parts.Length != 2) {
                        return BadRequest(new { message = "Invalid target format for toggleowner. Expected 'personId|gameId'." });
                    }

                    string personId = parts[0];
                    string gameId = parts[1];
                    bool state = updateDTO.Value.ToLower() == "true";

                    Game? game = gameList.Games.FirstOrDefault(g => g.Id == gameId);
                    if (game != null) {
                        Person? person = gameList.People.FirstOrDefault(p => p.Id == personId);
                        if (person != null) {
                            game.ToggleOwner(personId, state);
                        }
                        else {
                            return NotFound(new { message = "Person not found." });
                        }
                    }
                    else {
                        return NotFound(new { message = "Game not found." });
                    }
                }
                else {
                    return BadRequest(new { message = "Missing target or value for toggleowner." });
                }
                break;
            case "toggleexclude":
                if (!string.IsNullOrWhiteSpace(updateDTO.Target)) {
                    string gameId = updateDTO.Target;
                    bool state = updateDTO.Value?.ToLower() == "true";
                    Game? game = gameList.Games.FirstOrDefault(g => g.Id == gameId);
                    if (game != null) {
                        game.ToggleExclude(state);
                    }
                    else {
                        return NotFound(new { message = "Game not found." });
                    }
                }
                else {
                    return BadRequest(new { message = "Missing target for toggleexclude." });
                }
                break;

            default:
                return BadRequest(new { message = "Invalid action or target." });
        }
        await _gamesService.UpdateAsync(gameList);

        // Return the updated game list without the password
        return Ok(new {
            gameList.Id,
            gameList.Name,
            gameList.Games,
            gameList.People,
            gameList.RandomlyPickedGame
        });
    }
    
    // GET /api/lists/{id}/game
    // HEADER: Authorization: Bearer {token}
    // Randomly pick game
    // => Returns: Game object
    [HttpGet("{id}/game")]
    public async Task<IActionResult> GetRandomGame(string id) {
        // Validate the JWT token
        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        string? listId = _securityService.ValidateJwtToken(token);

        if (listId == null || listId != id)
            return Unauthorized(new { message = "Invalid token." });

        Models.GameList gameList = await _gamesService.GetByIdAsync(id);
        Game? game = gameList.GetRandomGame();
        
        // Update game list
        await _gamesService.UpdateAsync(gameList);

        if (game == null) {
            return NotFound(new { message = "No valid games." });
        }

        return Ok(game);
    }

    // DELETE /api/lists/{id}
    // HEADER: Authorization: Bearer {token}
    // Deletes a game list by its ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGameList(string id) {
        // Validate the JWT token
        string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        string? listId = _securityService.ValidateJwtToken(token);

        if (listId == null || listId != id) {
            return Unauthorized(new { message = "Invalid token." });
        }

        Models.GameList gameList = await _gamesService.GetByIdAsync(id);
        if (gameList == null) {
            return NotFound(new { message = "Game list not found." });
        }

        bool result = await _gamesService.DeleteListAsync(id);
        if (!result) {
            return BadRequest(new { message = "Failed to delete the game list." });
        }

        return Ok(new { message = "Game list deleted successfully." });
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Domain;
using MongoDB.Driver;

[ApiController]
[Route("thelist/api")]
public class GameListController : ControllerBase {
    private readonly IGameListService _gameListService;
    private readonly ISecurityService _securityService;
    private readonly IMapper _mapper;

    public GameListController(IGameListService gameListService, ISecurityService securityService, IMapper mapper) {
        _gameListService = gameListService;
        _securityService = securityService;
        _mapper = mapper;
    }

    [HttpPost("list/create")]
    public async Task<IActionResult> CreateList([FromBody] CreateGameListDto gameListDto) {
        if (gameListDto is null) {
            return BadRequest(new { message = "Malformed game list." });
        }

        GameList gameList = _mapper.Map<GameList>(gameListDto);
        try {
            await _gameListService.CreateListAsync(gameList);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }
        return Ok(new { listId = gameList.Id, token = _securityService.CreateAccessToken(gameList.Id) });
    }

    [HttpGet("list/{id}")]
    public async Task<IActionResult> GetList(string id) {
        // Validate request (check if token is valid)
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // Id check
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        GameList? gameList;
        try {
            gameList = await _gameListService.GetListByIdAsync(listId);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);

    }

    [HttpPost("list/{id}/games")]
    public async Task<IActionResult> AddGame(string id, [FromBody] CreateNamedEntityDto gameDto) {
        // Validate request
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // Id check
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        GameList? gameList;
        try {
            gameList = await _gameListService.GetListByIdAsync(listId);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }

        Game game = _mapper.Map<Game>(gameDto);
        gameList = await _gameListService.AddGameAsync(listId, game);

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);
    }

    [HttpDelete("list/{id}/games/{gameid}")]
    public async Task<IActionResult> DeleteGame(string id, string gameid) {
        // Validate request
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // Check if list id is valid
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        // Check if game id is valid
        if (!Guid.TryParse(gameid, out Guid gameId)) {
            return BadRequest(new { message = "Malformed game id." });
        }

        GameList? gameList;
        try {
            gameList = await _gameListService.RemoveGameAsync(listId, gameId);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);
    }

    [HttpPut("list/{id}/games/{gameid}/setexclude")]
    public async Task<IActionResult> SetExclusion(string id, string gameid, [FromBody] SetBooleanDto excludeDto) {
        // Validate request
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // Check if list id is valid
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        // Check if game id is valid
        if (!Guid.TryParse(gameid, out Guid gameId)) {
            return BadRequest(new { message = "Malformed game id." });
        }

        // Check if excludeDto.Value is a boolean
        if (excludeDto.Value.GetType() != typeof(bool)) {
            return BadRequest(new { message = "Malformed exclusion value." });
        }

        GameList? gameList;
        try {
            gameList = await _gameListService.SetGameExclusionAsync(listId, gameId, excludeDto.Value);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);
    }

    [HttpPut("list/{id}/games/{gameid}/owners/{personid}")]
    public async Task<IActionResult> SetOwner(string id, string gameid, string personid, [FromBody] SetBooleanDto ownerDto) {
        // Validate request
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // Check if list id is valid
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        // Check if game id is valid
        if (!Guid.TryParse(gameid, out Guid gameId)) {
            return BadRequest(new { message = "Malformed game id." });
        }

        // Check if person id is valid
        if (!Guid.TryParse(personid, out Guid personId)) {
            return BadRequest(new { message = "Malformed person id." });
        }

        // Check if excludeDto.Value is a boolean
        if (ownerDto.Value.GetType() != typeof(bool)) {
            return BadRequest(new { message = "Malformed exclusion value." });
        }

        GameList? gameList;
        try {
            gameList = await _gameListService.SetGameOwnershipAsync(listId, gameId, personId, ownerDto.Value);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);
    }

    [HttpGet("list/{id}/games/random")]
    public async Task<IActionResult> GetRandomGame(string id) {
        // Validate request
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // Id check
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        Game? game;
        try {
            game = await _gameListService.PickRandomGameAsync(listId);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }

        GameDto? gameDto = _mapper.Map<GameDto>(game);
        return Ok(gameDto);
    }

    [HttpPost("list/{id}/people")]
    public async Task<IActionResult> AddPerson(string id, [FromBody] CreateNamedEntityDto personDto) {
        // Validate request
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // Id check
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        GameList? gameList;
        try {
            gameList = await _gameListService.GetListByIdAsync(listId);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }

        Person person = _mapper.Map<Person>(personDto);
        gameList = await _gameListService.AddPersonAsync(listId, person);

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);
    }

    [HttpDelete("list/{id}/people/{personid}")]
    public async Task<IActionResult> DeletePerson(string id, string personid) {
        // Validate request
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // List id check
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        // Person id check
        if (!Guid.TryParse(personid, out Guid personId)) {
            return BadRequest(new { message = "Malformed person id." });
        }

        GameList? gameList;
        try {
            gameList = await _gameListService.RemovePersonAsync(listId, personId);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }
        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);
    }

    [HttpDelete("list/{id}")]
    public async Task<IActionResult> DeleteList(string id) {
        // Validate request
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // List id check
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        try {
            await _gameListService.DeleteListAsync(listId);
        }
        catch (Exception ex) {
            return NotFound(new { message = ex.Message });
        }

        return NoContent();
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto) {
        // Check if list exists
        GameList? gameList = await _gameListService.GetListByNameAsync(loginDto.Name);

        if (gameList is null) {
            return Unauthorized(new { message = "Incorrect name or password." });
        }

        // Check if password is correct
        if (!_securityService.ValidatePassword(loginDto.Password, gameList.Password)) {
            return Unauthorized(new { message = "Incorrect name or password." });
        }

        return Ok(new LoginResponseDto {
            AccessToken = _securityService.CreateAccessToken(gameList.Id)
        });
    }
}
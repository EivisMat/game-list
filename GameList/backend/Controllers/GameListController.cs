using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Domain;

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
            return BadRequest(new { message = ex.Message });
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
            return BadRequest(new { message = ex.Message });
        }

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);

    }

    [HttpPost("list/{id}/games")]
    public async Task<IActionResult> AddGame(string id, [FromBody] CreateNamedEntityDto gameDto) {
        // Validate request
        //AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        //if (!result.IsValid) {
        //    return Unauthorized(new { message = result.Message });
        //}

        // Id check
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        GameList? gameList;
        try {
            gameList = await _gameListService.GetListByIdAsync(listId);
        }
        catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
        if (gameList is null) {
            return NotFound(new { message = "Game list not found." });
        }

        Game game = _mapper.Map<Game>(gameDto);
        gameList = await _gameListService.AddGameAsync(listId, game);

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);
    }
}
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

        await _gameListService.CreateListAsync(gameList);

        return Ok(new { message = "Game list created successfully." });
    }

    [HttpGet("list/{id}")]
    public async Task<IActionResult> GetList(string id) {
        // Token check
        AuthValidationResult result = _securityService.ValidateHttpRequest(Request);
        if (!result.IsValid) {
            return Unauthorized(new { message = result.Message });
        }

        // Id check
        if (!Guid.TryParse(id, out Guid listId)) {
            return BadRequest(new { message = "Malformed list id." });
        }

        GameList? gameList = await _gameListService.GetListByIdAsync(listId);
        if (gameList is null) {
            return NotFound(new { message = "Game list not found." });
        }

        GameListDto gameListDto = _mapper.Map<GameListDto>(gameList);

        return Ok(gameListDto);
    }
}
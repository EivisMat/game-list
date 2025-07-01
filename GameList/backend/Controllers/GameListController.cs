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
            throw new ArgumentNullException(nameof(gameListDto), "Game list cannot be null.");
        }

        GameList gameList = _mapper.Map<GameList>(gameListDto);

        await _gameListService.CreateListAsync(gameList);

        return Ok(new { message = "Game list created successfully." });
    }
}
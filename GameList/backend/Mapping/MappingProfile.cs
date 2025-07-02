using AutoMapper;
using Models.DTOs;
using Models.Domain;
using Models.Document;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<CreateGameListDto, GameList>();
        CreateMap<CreateNamedEntityDto, Person>();
        CreateMap<CreateNamedEntityDto, Game>();

        CreateMap<GameList, CreateGameListDto>();
        CreateMap<Person, CreateNamedEntityDto>();
        CreateMap<Game, CreateNamedEntityDto>();

        CreateMap<GameList, GameListDocument>();
        CreateMap<Game, GameDocument>();
        CreateMap<Person, PersonDocument>();

        CreateMap<GameListDocument, GameList>();
        CreateMap<GameDocument, Game>();
        CreateMap<PersonDocument, Person>();
    }
}
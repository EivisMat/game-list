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
        CreateMap<Game, GameDocument>()
            .ForMember(dest => dest.Owners,
                opt => opt.MapFrom(src => src.Owners.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value)));
        CreateMap<Person, PersonDocument>();

        CreateMap<GameListDocument, GameList>();
        CreateMap<GameDocument, Game>()
            .ForMember(dest => dest.Owners,
               opt => opt.MapFrom(src => src.Owners.ToDictionary(kvp => Guid.Parse(kvp.Key), kvp => kvp.Value)));
        CreateMap<PersonDocument, Person>();

        CreateMap<GameList, GameListDto>();
        CreateMap<GameListDto, GameList>();

        CreateMap<Game, GameDto>();
        CreateMap<GameDto, Game>();

        CreateMap<Person, PersonDto>();
        CreateMap<PersonDto, Person>();
    }
}
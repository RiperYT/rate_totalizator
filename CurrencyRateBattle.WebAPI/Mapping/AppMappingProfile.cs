using AutoMapper;
using CurrencyRateBattle.WebAPI.Data.Entities;
using CurrencyRateBattle.WebAPI.Dtos;


namespace CurrencyRateBattle.WebAPI.Mapping;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        _ = CreateMap<CreateRoomDto, Room>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.TypeGame));
        _ = CreateMap<Room, RoomDto>().ForMember(dest => dest.TypeGame, opt => opt.MapFrom(src => src.TypeId));
        _ = CreateMap<UserRegistrationDto, User>().ReverseMap();
        _ = CreateMap<UserDto, User>().ReverseMap();
        _ = CreateMap<CurrencyDto, Currency>().ReverseMap();
        _ = CreateMap<RoomUserDto, RoomUser>().ReverseMap();
    }
}

using AutoMapper;
using TLM.Books.Application.Features.UserFeature.Commands;
using TLM.Books.Application.Models;
using TLM.Books.Domain.Entities;

namespace TLM.Books.Application.Profiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<CreateUserCommand, User>();
        CreateMap<User, UserView>()
            .ForMember(des => des.BookViews,
                opt=> opt.MapFrom(src => src.Books));
        CreateMap<UpdateUserCommand, User>()
            .ForAllMembers(opts => 
                opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
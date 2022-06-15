using AutoMapper;
using TLM.Books.Application.Features.BookFeature.Commands;
using TLM.Books.Application.Models;
using TLM.Books.Domain.Entities;

namespace TLM.Books.Application.Profiles;

public class BookProfiles : Profile
{
    public BookProfiles()
    {
        CreateMap<CreateBookCommand, Book>();
        CreateMap<Book, BookView>();
        CreateMap<UpdateBookCommand, Book>()
            .ForAllMembers(opts => 
                opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
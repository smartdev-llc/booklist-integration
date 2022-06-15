using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;
using TLM.Books.Domain.Entities;

namespace TLM.Books.Application.Features.BookFeature.Queries;

public class GetAllBooksQuery : IRequest<MethodResult<IEnumerable<BookView>>>
{
    
}

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, MethodResult<IEnumerable<BookView>>>
{
    private readonly IBookDbContext _context;
    public GetAllBooksQueryHandler(IBookDbContext context)
    {
        _context = context;
    }
    public async Task<MethodResult<IEnumerable<BookView>>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<IEnumerable<BookView>>();
        var queryable = _context.Books.AsQueryable();
        var bookList = await queryable.Select(x => new BookView
        {
            Name = x.Name,
            Author = x.Author,
            ISBN = x.ISBN,
            Id = x.Id
        }).ToListAsync(cancellationToken: cancellationToken);
        methodResult.StatusCode = StatusCodes.Status200OK;
        methodResult.Result = bookList.AsReadOnly();
        return methodResult;
    }
}
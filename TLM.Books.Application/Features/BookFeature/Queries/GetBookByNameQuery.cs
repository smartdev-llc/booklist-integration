using AspNetCore.IQueryable.Extensions.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;
using TLM.Books.Common.Models;

namespace TLM.Books.Application.Features.BookFeature.Queries;

public class GetBookByNameQuery : IRequest<MethodResult<IEnumerable<BookView>>>
{
    public string Name { get; set; }
}

public class GetBookByNameQueryHandler : IRequestHandler<GetBookByNameQuery, MethodResult<IEnumerable<BookView>>>
{
    private readonly IBookDbContext _context;
    public GetBookByNameQueryHandler(IBookDbContext context)
    {
        _context = context;
    }
    public async Task<MethodResult<IEnumerable<BookView>>> Handle(GetBookByNameQuery query, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<IEnumerable<BookView>>();
        var queryable = _context.Books.AsQueryable();
        var bookSearch = new BookSearch
        {
            Name = query.Name
        };
        var bookList = await queryable.Select(x => new BookView
            {
                Name = x.Name,
                Author = x.Author,
                ISBN = x.ISBN,
                Id = x.Id
            }).Filter(bookSearch)
            .ToListAsync(cancellationToken: cancellationToken);
        methodResult.StatusCode = StatusCodes.Status200OK;
        methodResult.Result = bookList.AsReadOnly();

        return methodResult;
    }
}
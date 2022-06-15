using AspNetCore.IQueryable.Extensions.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;
using TLM.Books.Common.Models;

namespace TLM.Books.Application.Features.UserFeature.Queries;

public class GetUserByIdQuery : IRequest<MethodResult<UserView>>
{
    public Guid Id { get; set; }
}

public class GetBookByNameQueryHandler : IRequestHandler<GetUserByIdQuery, MethodResult<UserView>>
{
    private readonly IBookDbContext _context;
    public GetBookByNameQueryHandler(IBookDbContext context)
    {
        _context = context;
    }
    public async Task<MethodResult<UserView>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<UserView>();
        var queryable = _context.Users.AsQueryable();
        queryable = queryable.Include(x => x.Books);
        
        var userView = await queryable.Select(x => new UserView
        {
            Name = x.Name,
            Id = x.Id,
            BookViews = x.Books.Select(y => new BookView
            {
                Id = y.Id,
                Author = y.Author,
                Name = y.Name,
                ISBN = y.ISBN
            })
        }).FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken: cancellationToken);
        methodResult.StatusCode = StatusCodes.Status200OK;
        methodResult.Result = userView;
        return methodResult;
    }
}
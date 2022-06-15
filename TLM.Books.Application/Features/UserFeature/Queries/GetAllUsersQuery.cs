using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;

namespace TLM.Books.Application.Features.UserFeature.Queries;

public class GetAllUsersQuery : IRequest<MethodResult<IEnumerable<UserView>>>
{
    
}

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, MethodResult<IEnumerable<UserView>>>
{
    private readonly IBookDbContext _context;
    public GetAllUsersQueryHandler(IBookDbContext context)
    {
        _context = context;
    }
    public async Task<MethodResult<IEnumerable<UserView>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<IEnumerable<UserView>>();
        var queryable = _context.Users.AsQueryable();
        queryable = queryable.Include(x => x.Books);
        
        var userList = await queryable.Select(x => new UserView
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
        }).ToListAsync(cancellationToken: cancellationToken);
        methodResult.StatusCode = StatusCodes.Status200OK;
        methodResult.Result = userList.AsReadOnly();
        return methodResult;
    }
}
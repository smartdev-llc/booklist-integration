using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;

namespace TLM.Books.Application.Features.UserFeature.Commands;

public class AddBookToUserComand : AddBookToUserRequest, IRequest<MethodResult<UserView>>
{
    
}

public class AddBookToUserComandHandler : IRequestHandler<AddBookToUserComand, MethodResult<UserView>>
{
    private readonly IBookDbContext _context;
    private readonly IMapper _mapper;

    public AddBookToUserComandHandler(IBookDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<MethodResult<UserView>> Handle(AddBookToUserComand request, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<UserView>();
        var user = await _context.Users
            .Include(x => x.Books)
            .FirstOrDefaultAsync(a => a.Id == request.UserId, cancellationToken: cancellationToken);

        methodResult.StatusCode = StatusCodes.Status200OK;
        methodResult.Result = default;
        if (user != null)
        {
            var books = await _context.Books.Where(x => request.BookIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
            if (books.Any())
            {
                var exitedBooks = user.Books.Select(x => x.Id).ToList();
                var newBooks = books.Where(x => !exitedBooks.Contains(x.Id));
                if (newBooks.Any())
                {
                    foreach (var newBook in newBooks)
                    {
                        user.Books.Add(newBook);
                    }
                }
            }
            await _context.SaveChangesAsync();
            var view = _mapper.Map<UserView>(user);
            methodResult.Result = view;
        }

        return methodResult;
    }
}
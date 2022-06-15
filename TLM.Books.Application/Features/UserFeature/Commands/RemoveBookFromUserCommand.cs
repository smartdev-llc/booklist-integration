using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;

namespace TLM.Books.Application.Features.UserFeature.Commands;

public class RemoveBookFromUserCommand : RemoveBookFromUserRequest, IRequest<MethodResult<UserView>>
{
    
}

public class RemoveBookFromUserCommandHandler : IRequestHandler<RemoveBookFromUserCommand, MethodResult<UserView>>
{
    private readonly IBookDbContext _context;
    private readonly IMapper _mapper;

    public RemoveBookFromUserCommandHandler(IBookDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<MethodResult<UserView>> Handle(RemoveBookFromUserCommand request, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<UserView>();
        var user = await _context.Users
            .Include(x => x.Books)
            .FirstOrDefaultAsync(a => a.Id == request.UserId, cancellationToken: cancellationToken);

        methodResult.StatusCode = StatusCodes.Status204NoContent;
        methodResult.Result = default;
        if (user != null)
        {
            var books = await _context.Books.Where(x => request.BookIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
            if (books.Any())
            {
                var exitedBooks = user.Books.Select(x => x.Id).ToList();
                var needToRemoveBooks = books.Where(x => exitedBooks.Contains(x.Id));
                if (needToRemoveBooks.Any())
                {
                    foreach (var book in needToRemoveBooks)
                    {
                        user.Books.Remove(book);
                    }
                }
            }
            await _context.SaveChangesAsync();
            var view = _mapper.Map<UserView>(user);
            methodResult.Result = view;
            methodResult.StatusCode = StatusCodes.Status200OK;
        }

        return methodResult;
    }
}
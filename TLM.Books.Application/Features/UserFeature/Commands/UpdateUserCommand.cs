using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;

namespace TLM.Books.Application.Features.UserFeature.Commands;

public class UpdateUserCommand : UpdateUserRequest, IRequest<MethodResult<UserView>>
{
    
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, MethodResult<UserView>>
{
    private readonly IBookDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IBookDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<MethodResult<UserView>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<UserView>();
        var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);

        methodResult.StatusCode = StatusCodes.Status204NoContent;
        methodResult.Result = default;
        if (user != null)
        {
            _mapper.Map(request, user);
            await _context.SaveChangesAsync();
            var bookView = _mapper.Map<UserView>(user);
            methodResult.Result = bookView;
            methodResult.StatusCode = StatusCodes.Status200OK;
        }

        return methodResult;
    }
}
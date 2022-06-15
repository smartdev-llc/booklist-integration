using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;

namespace TLM.Books.Application.Features.UserFeature.Commands;

public class DeleteUserCommand : DeleteUserRequest, IRequest<MethodResult<UserView>>
{
    
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, MethodResult<UserView>>
{
    private readonly IBookDbContext _context;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IBookDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<MethodResult<UserView>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<UserView>();
        var user = _context.Users.FirstOrDefault(a => a.Id == request.Id);

        methodResult.StatusCode = StatusCodes.Status204NoContent;
        methodResult.Result = default;
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            var view = _mapper.Map<UserView>(user);
            methodResult.Result = view;
            methodResult.StatusCode = StatusCodes.Status200OK;
        }

        return methodResult;
    }
}
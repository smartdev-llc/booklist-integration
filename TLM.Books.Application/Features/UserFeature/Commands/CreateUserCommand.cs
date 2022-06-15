using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TLM.Books.Application.Features.BookFeature.Commands;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;
using TLM.Books.Domain.Entities;

namespace TLM.Books.Application.Features.UserFeature.Commands;

public class CreateUserCommand : CreateUserRequest, IRequest<MethodResult<UserView>>
{
    
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, MethodResult<UserView>>
{
    private readonly IBookDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IBookDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<MethodResult<UserView>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<UserView>();
        var userEntity = _mapper.Map<User>(request);
        await _context.Users.AddAsync(userEntity, cancellationToken);
        await _context.SaveChangesAsync();
        methodResult.Result =  _mapper.Map<UserView>(userEntity);
        methodResult.StatusCode = StatusCodes.Status200OK;
        return methodResult;
    }
}
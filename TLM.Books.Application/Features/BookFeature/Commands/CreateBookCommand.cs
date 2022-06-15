using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;
using TLM.Books.Domain.Entities;

namespace TLM.Books.Application.Features.BookFeature.Commands;

public class CreateBookCommand : CreateBookRequest, IRequest<MethodResult<BookView>>
{
    
}

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, MethodResult<BookView>>
{
    private readonly IBookDbContext _context;
    private readonly IMapper _mapper;

    public CreateBookCommandHandler(IBookDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<MethodResult<BookView>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<BookView>();
        var bookEntity = _mapper.Map<Book>(request);
        await _context.Books.AddAsync(bookEntity, cancellationToken);
        await _context.SaveChangesAsync();
        methodResult.Result =  _mapper.Map<BookView>(bookEntity);
        methodResult.StatusCode = StatusCodes.Status200OK;
        return methodResult;
    }
}
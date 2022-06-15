using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TLM.Books.Application.Interfaces;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;
using TLM.Books.Domain.Entities;

namespace TLM.Books.Application.Features.BookFeature.Commands;

public class DeleteBookCommand : DeleteBookRequest, IRequest<MethodResult<BookView>>
{
    
}

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, MethodResult<BookView>>
{
    private readonly IBookDbContext _context;
    private readonly IMapper _mapper;

    public DeleteBookCommandHandler(IBookDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<MethodResult<BookView>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var methodResult = new MethodResult<BookView>();
        var book = _context.Books.FirstOrDefault(a => a.Id == request.Id);

        methodResult.StatusCode = StatusCodes.Status200OK;
        methodResult.Result = default;
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            var bookView = _mapper.Map<BookView>(book);
            methodResult.Result = bookView;
        }

        return methodResult;
    }
}
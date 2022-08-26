namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors.Services;
using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class AddAuthor
{
    public class Command : IRequest<AuthorDto>
    {
        public readonly AuthorForCreationDto AuthorToAdd;

        public Command(AuthorForCreationDto authorToAdd)
        {
            AuthorToAdd = authorToAdd;
        }
    }

    public class Handler : IRequestHandler<Command, AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IAuthorRepository authorRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthorDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var author = Author.Create(request.AuthorToAdd);
            await _authorRepository.Add(author, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var authorAdded = await _authorRepository.GetById(author.Id, cancellationToken: cancellationToken);
            return _mapper.Map<AuthorDto>(authorAdded);
        }
    }
}
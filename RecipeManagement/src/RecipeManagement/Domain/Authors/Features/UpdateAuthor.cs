namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors;
using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors.Validators;
using RecipeManagement.Domain.Authors.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using MapsterMapper;
using MediatR;

public static class UpdateAuthor
{
    public class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly AuthorForUpdateDto AuthorToUpdate;

        public Command(Guid author, AuthorForUpdateDto newAuthorData)
        {
            Id = author;
            AuthorToUpdate = newAuthorData;
        }
    }

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var authorToUpdate = await _authorRepository.GetById(request.Id, cancellationToken: cancellationToken);

            authorToUpdate.Update(request.AuthorToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}
namespace RecipeManagement.Domain.Authors.Features;

using RecipeManagement.Domain.Authors.Dtos;
using RecipeManagement.Domain.Authors.Services;
using RecipeManagement.Wrappers;
using SharedKernel.Exceptions;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetAuthorList
{
    public class Query : IRequest<PagedList<AuthorDto>>
    {
        public readonly AuthorParametersDto QueryParameters;

        public Query(AuthorParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public class Handler : IRequestHandler<Query, PagedList<AuthorDto>>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;

        public Handler(IAuthorRepository authorRepository, IMapper mapper, SieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<PagedList<AuthorDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var collection = _authorRepository.Query();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "Id",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<AuthorDto>();

            return await PagedList<AuthorDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}
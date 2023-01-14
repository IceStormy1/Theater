using AutoMapper;

namespace Theater.Core
{
    public abstract class ServiceBase<TRepository>
    {
        protected readonly IMapper Mapper;
        protected readonly TRepository Repository;

        protected ServiceBase(IMapper mapper, TRepository repository)
        {
            Mapper = mapper;
            Repository = repository;
        }
    }
}

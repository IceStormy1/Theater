using AutoMapper;
using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Contracts;
using Theater.Entities;

namespace Theater.Core
{
    public abstract class ServiceBase<TModel, TEntity> : ICrudService<TModel, TEntity>
        where TEntity : class, IEntity
        where TModel : class
    {
        protected readonly IMapper Mapper;
        protected readonly ICrudRepository<TEntity> Repository;

        protected ServiceBase(IMapper mapper, ICrudRepository<TEntity> repository)
        {
            Mapper = mapper;
            Repository = repository;
        }

        public async Task<WriteResult<TModel>> GetById(Guid id)
        {
            var entity = await Repository.GetByEntityId(id);

            return entity is null
                ? WriteResult<TModel>.FromError(PieceErrors.NotFound.Error)
                : WriteResult.FromValue(Mapper.Map<TModel>(entity));
        }

        public async Task<WriteResult<DocumentMeta>> CreateOrUpdate(TModel model, Guid? pieceId)
        {
            var piece = await Repository.GetByEntityId(pieceId ?? Guid.NewGuid());

            if (piece is null)
            {
                piece = Mapper.Map<TEntity>(model);

                await Repository.Add(piece);
            }

            Mapper.Map(model, piece);

            await Repository.Update(piece);

            return WriteResult.FromValue(new DocumentMeta(piece.Id));
        }

        public async Task<WriteResult> Delete(Guid id)
        {
            await Repository.Delete(id);

            return WriteResult.Successful;
        }
    }
}

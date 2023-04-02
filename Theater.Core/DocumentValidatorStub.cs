using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Common;

namespace Theater.Core
{
    public class  DocumentValidatorStub<T> : IDocumentValidator<T> where T : class
    {
        public Task<WriteResult> CheckIfCanCreate(T parameters) => Task.FromResult(WriteResult.Successful);

        public Task<WriteResult> CheckIfCanUpdate(Guid entityId, T parameters) => Task.FromResult(WriteResult.Successful);

        public Task<WriteResult> CheckIfCanDelete(Guid entityId) => Task.FromResult(WriteResult.Successful);
    }
}

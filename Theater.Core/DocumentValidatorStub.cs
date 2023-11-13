using System;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Common;

namespace Theater.Core;

public sealed class  DocumentValidatorStub<T> : IDocumentValidator<T> where T : class
{
    public Task<Result> CheckIfCanCreate(T parameters, Guid? userId = null) => Task.FromResult(Result.Successful);

    public Task<Result> CheckIfCanUpdate(Guid entityId, T parameters, Guid? userId = null) => Task.FromResult(Result.Successful);

    public Task<Result> CheckIfCanDelete(Guid entityId, Guid? userId = null) => Task.FromResult(Result.Successful);
}
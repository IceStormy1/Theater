using System;

namespace Theater.Contracts;

public class DocumentMeta
{
    public DocumentMeta(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; }
}
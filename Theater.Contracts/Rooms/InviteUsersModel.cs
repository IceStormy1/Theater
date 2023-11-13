using System;
using System.Collections.Generic;

namespace Theater.Contracts.Rooms;

public sealed class InviteUsersModel
{
    /// <summary>
    /// Идентификаторы пользователей, которые были приглашены в чат
    /// </summary>
    public IReadOnlyCollection<Guid> InvitedUsersIds { get; set; }
}
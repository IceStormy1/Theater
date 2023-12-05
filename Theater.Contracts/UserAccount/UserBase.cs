using System;

namespace Theater.Contracts.UserAccount;

public abstract class UserBase : IUser
{
    /// <inheritdoc cref="IUser.FirstName"/>
    public string FirstName { get; set; }

    /// <inheritdoc cref="IUser.LastName"/>
    public string LastName { get; set; }

    /// <inheritdoc cref="IUser.MiddleName"/>
    public string MiddleName { get; set; }

    /// <inheritdoc cref="IUser.Gender"/>
    public GenderType Gender { get; set; }

    /// <inheritdoc cref="IUser.BirthDate"/>
    public DateTime BirthDate { get; set; }
}
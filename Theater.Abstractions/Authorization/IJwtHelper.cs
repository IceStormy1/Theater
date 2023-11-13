using Theater.Entities.Users;

namespace Theater.Abstractions.Authorization;

public interface IJwtHelper
{
    string GenerateJwtToken(UserEntity user);
}
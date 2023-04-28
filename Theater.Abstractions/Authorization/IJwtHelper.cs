using Theater.Entities.Authorization;

namespace Theater.Abstractions.Authorization;

public interface IJwtHelper
{
    string GenerateJwtToken(UserEntity user);
}
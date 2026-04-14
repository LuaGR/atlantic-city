using AtlanticCity.Application.DTOs;

namespace AtlanticCity.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
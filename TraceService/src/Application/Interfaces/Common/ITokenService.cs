using System;
using System.Security.Claims;

namespace TraceService.Application.Interfaces
{
    public interface ITokenService
    {
        (string token, DateTime expiry) GenerateToken(Guid id);
        (string token, DateTime expiry) GenerateRefreshToken();
        Guid? GetTokenId(string token);
        public ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}

using System;

namespace TraceService.Application.Models
{
    public class AuthenticationModel
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }

    public class AuthenticationModel<T> where T : class
    {
        public T Data { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}

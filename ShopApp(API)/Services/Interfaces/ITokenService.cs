using ShopApp_API_.Entities;

namespace ShopApp_API_.Services.Interfaces
{
    public interface ITokenService
    {
        string GetToken(string secretKey, string audience, string issuer, AppUser user, IList<string> roles);
    }
}

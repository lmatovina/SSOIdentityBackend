using Duende.IdentityServer.Models;

namespace SSO.IdentityServer.Config;

public static class ScopeConfig
{
    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new ApiScope("api1", "Glavni API")
    };
}

using Duende.IdentityServer.Models;
using System.Collections.Generic;

namespace SSO.IdentityServer.Config;

public static class ClientConfig
{
	public static IEnumerable<Client> Clients => new List<Client>
	{
		new Client
		{
			ClientId = "angular_client",
			ClientName = "Angular Frontend",
			AllowedGrantTypes = GrantTypes.Code,
			RequireClientSecret = false,
			RedirectUris = { "http://localhost:4200/auth-callback" },
			PostLogoutRedirectUris = { "http://localhost:4200/" },
			AllowedScopes = { "openid", "profile", "api1" },
			RequirePkce = true,
			AllowAccessTokensViaBrowser = true
		}
	};
}

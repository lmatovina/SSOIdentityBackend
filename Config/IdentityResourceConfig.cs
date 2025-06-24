using Duende.IdentityServer.Models;
using System.Collections.Generic;

namespace SSO.IdentityServer.Config;

public static class IdentityResourceConfig
{
	public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
	{
		new IdentityResources.OpenId(),
		new IdentityResources.Profile()
	};
}

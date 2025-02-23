using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Auth
{
    public class DiscordAuthConstants : IAuthConstants
    {
        public string ClientId => IAuthConstants.Read("DISCORD_CLIENT_ID");

        public string ClientSecret => IAuthConstants.Read("DISCORD_CLIENT_SECRET");

        // Unused for Discord
        public string TenantId => string.Empty;
        public string Instance => string.Empty;
        public string Domain => string.Empty;
        public string Authority => "https://discord.com/api/oauth2/authorize";

        public string CallbackPath => "/signin-discord";

        public string AuthenticationScheme => "Discord";

        public List<string> Scopes => ["identify"];

        public bool UseOpenIdConnect => false;
    }
}

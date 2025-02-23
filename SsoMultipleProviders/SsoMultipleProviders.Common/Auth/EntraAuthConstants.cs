using Microsoft.AspNetCore.Authentication.OAuth;
using SsoMultipleProviders.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Auth
{
    public class EntraAuthConstants : IAuthConstants
    {
        public string ClientId => IAuthConstants.Read("AZURE_AD_CLIENT_ID");
        public string ClientSecret => IAuthConstants.Read("AZURE_AD_CLIENT_SECRET");
        public string TenantId => IAuthConstants.Read("AZURE_AD_TENANT_ID");
        public string Instance => IAuthConstants.Read("AZURE_AD_INSTANCE");
        public string Domain => IAuthConstants.Read("AZURE_AD_DOMAIN");
        public string Authority => Instance + TenantId;
        public string CallbackPath => "/signin-oidc";
        public string AuthenticationScheme => "AzureEntraId";
        public List<string> Scopes => ["openid"];
        public bool UseOpenIdConnect => true;
    }
}

using SsoMultipleProviders.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Auth
{
    public interface IAuthConstants
    {
        string ClientId { get; }
        string ClientSecret { get; }
        string TenantId { get; }
        string Instance { get; }
        bool UseOpenIdConnect { get; }
        string Domain { get; }
        string Authority { get; }
        string CallbackPath { get; }
        string AuthenticationScheme { get; }
        List<string> Scopes { get; }
        public static string Issuer => "SET_TO_YOUR_ISSUER";
        public static string Audience => "SET_TO_YOUR_AUDIENCE";
        public static string JwtSecret => Read("JWT_SHARED_SECRET_KEY");
        public static string Read(string name)
        {
            return EnvironmentVariableHelper.ReadEnvironmentVariable(name);
        }
    }
}

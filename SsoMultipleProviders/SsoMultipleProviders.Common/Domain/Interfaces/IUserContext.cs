using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Domain.Interfaces
{
    public interface IUserContext<TUser>
    {
        TUser? CurrentUser { get; }
    }
}

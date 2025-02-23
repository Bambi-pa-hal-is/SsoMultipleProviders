using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Domain.Interfaces
{
    public interface ITimeProvider
    {
        DateTime GetCurrentTime();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Domain
{
    public interface IEntity<TId> where TId : struct
    {
        public TId Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class TokenComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<long, string> dicToken;

    }
 
}

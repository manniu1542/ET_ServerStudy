using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{

    [ChildType(typeof(RoleInfo))]
    [ComponentOf(typeof(Scene))]
    public class RoleInfoComponent : Entity, IAwake, IDestroy
    {

        public RoleInfo roleInfo;
        


    }
}

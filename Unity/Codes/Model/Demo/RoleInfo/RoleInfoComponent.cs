using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{

    /// <summary>
    /// 账号角色信息组件
    /// </summary>
    [ChildType(typeof(RoleInfo))]
    [ComponentOf(typeof(Scene))]
    public class RoleInfoComponent : Entity, IAwake, IDestroy
    {

        public RoleInfo roleInfo;
        


    }
}

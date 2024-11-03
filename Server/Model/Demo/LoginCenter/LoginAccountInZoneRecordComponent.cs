using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    /// <summary>
    /// 登陆的账号所在的区服记录组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class LoginAccountInZoneRecordComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<long, int> dicLoginAccountZone;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    /// <summary>
    /// 服务区信息
    /// </summary>
    [ChildType(typeof(ServerInfo))]
    [ComponentOf(typeof(Scene))]
    public class ServerInfoComponent : Entity, IAwake, IDestroy
    {

        public List<ServerInfo> ServerInfoList;

        public int curServerId;
    }
}

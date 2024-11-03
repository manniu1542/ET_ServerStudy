using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class ServerInfoManager : Entity, IAwake, IDestroy
    {

        public List<ServerInfo> ServerInfoList;

    }
}

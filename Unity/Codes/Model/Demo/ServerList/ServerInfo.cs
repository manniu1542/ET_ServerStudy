using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum ServerInfoState
    {
        Normal,
        Off_The_Line,
    }

    [ComponentOf()]
    public class ServerInfo : Entity, IAwake, IDestroy
    {

        public string Name;

        public ServerInfoState State;
    }
}

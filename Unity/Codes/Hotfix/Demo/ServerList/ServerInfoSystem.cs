using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class ServerInfoAwakeSystem : AwakeSystem<ServerInfo>
    {
        public override void Awake(ServerInfo self)
        {

        }
    }

    [ObjectSystem]
    public class ServerInfoDestroySystem : DestroySystem<ServerInfo>
    {
        public override void Destroy(ServerInfo self)
        {

        }
    }

    [FriendClass(typeof(ServerInfo))]
    public static class ServerInfoSystem
    {

        public static void FromMessage(this ServerInfo self, MServerInfo m)
        {
          
            self.Id = m.ServerId;
            self.Name = m.Name;
            self.State = (ServerInfoState)m.State;
        }

        public static MServerInfo ToMessage(this ServerInfo self)
        {
            return new MServerInfo()
            {
                ServerId = self.Id,
                Name = self.Name,
                State = (int)self.State
            };
        }



    }
}

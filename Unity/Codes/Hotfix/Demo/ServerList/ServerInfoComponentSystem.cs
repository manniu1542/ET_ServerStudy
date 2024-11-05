using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class ServerInfoComponentAwakeSystem : AwakeSystem<ServerInfoComponent>
    {
        public override void Awake(ServerInfoComponent self)
        {
            self.ServerInfoList = new();
        }
    }

    [ObjectSystem]
    public class ServerInfoComponentDestroySystem : DestroySystem<ServerInfoComponent>
    {
        public override void Destroy(ServerInfoComponent self)
        {

        }
    }

    [FriendClass(typeof(ServerInfoComponent))]
    public static class ServerInfoComponentSystem
    {

        public static void SetServerInfoList(this ServerInfoComponent self, List<MServerInfo> ListServerInfo)
        {
            foreach (var item in self.ServerInfoList)
            {
                item.Dispose();
            }
            self.ServerInfoList.Clear();

            foreach (var item in ListServerInfo)
            {
                ServerInfo si = self.AddChildWithId<ServerInfo>(item.ServerId);
                si.FromMessage(item);
                self.ServerInfoList.Add(si);
            }



        }



    }
}

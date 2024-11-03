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

        public static void ReadAccountInfo(this ServerInfo self, long accountId, string token)
        {




        }



    }
}

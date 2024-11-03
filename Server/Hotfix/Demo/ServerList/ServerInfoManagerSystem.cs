using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class ServerInfoManagerAwakeSystem : AwakeSystem<ServerInfoManager>
    {
        public override void Awake(ServerInfoManager self)
        {

        }
    }

    [ObjectSystem]
    public class ServerInfoManagerDestroySystem : DestroySystem<ServerInfoManager>
    {
        public override void Destroy(ServerInfoManager self)
        {

        }
    }

    [FriendClass(typeof(ServerInfoManager))]
    public static class ServerInfoManagerSystem
    {

        public static void ReadAccountInfo(this ServerInfoManager self)
        {




        }



    }
}

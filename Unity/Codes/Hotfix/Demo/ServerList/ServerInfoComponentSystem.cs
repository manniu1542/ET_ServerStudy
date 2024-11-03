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

        public static void ReadAccountInfo(this ServerInfoComponent self)
        {




        }



    }
}

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
            self.InitServerInfo();
        }
    }

    [ObjectSystem]
    public class ServerInfoManagerDestroySystem : DestroySystem<ServerInfoManager>
    {
        public override void Destroy(ServerInfoManager self)
        {

        }
    }
    [FriendClass(typeof(ServerInfo))]
    [FriendClass(typeof(ServerInfoManager))]
    public static class ServerInfoManagerSystem
    {

        public static async void InitServerInfo(this ServerInfoManager self)
        {
            DBComponent dBComponent = DBManagerComponent.Instance.GetZoneDB(self.DomainZone());
            List<ServerInfo> listServerInfo = await dBComponent.Query<ServerInfo>(_ => true);

            //数据库没有 存储ServerInfo，读表并写入数据库
            if (listServerInfo == null || listServerInfo.Count == 0)
            {
                Log.Error("数据库中没有区服列表，需要存储 区服列表");
                listServerInfo = listServerInfo ?? new List<ServerInfo>();
                foreach (var item in ServerInfoConfigCategory.Instance.GetAll())
                {
                    ServerInfo newServerInfo = self.AddChildWithId<ServerInfo>(item.Key);
                    newServerInfo.State = ServerInfoState.Normal;
                    newServerInfo.Name = item.Value.ServerName;
                    listServerInfo.Add(newServerInfo);

                    await dBComponent.Save(newServerInfo);
                }

            }


            self.ServerInfoList = listServerInfo;

        }



    }
}

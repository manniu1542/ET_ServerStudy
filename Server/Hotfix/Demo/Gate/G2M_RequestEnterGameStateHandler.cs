using System;

namespace ET
{
    [FriendClass(typeof (RoleInfo))]
    public class G2M_RequestEnterGameStateHandler: AMActorLocationRpcHandler<Unit, G2M_RequestEnterGameState, M2G_RequestEnterGameState>
    {
        protected override async ETTask Run(Unit unit, G2M_RequestEnterGameState request, M2G_RequestEnterGameState response, Action reply)
        {
            Log.Info("map服务器 告诉玩家 玩家下线了");

            //用map服务器 告诉玩家 玩家下线了
            // unit.DomainScene().GetComponent<UnitComponent>();
            
            
            reply();

            await ETTask.CompletedTask;
        }
    }
}
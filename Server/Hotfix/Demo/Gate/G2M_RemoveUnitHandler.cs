using System;

namespace ET
{
    [FriendClass(typeof (RoleInfo))]
    public class G2M_RemoveUnitHandler: AMActorLocationRpcHandler<Unit, G2M_RemoveUnit, M2G_RemoveUnit>
    {
        protected override async ETTask Run(Unit unit, G2M_RemoveUnit request, M2G_RemoveUnit response, Action reply)
        {
          
            //移除玩家的unit，保存玩家数据， locatin定位服务器也移除 玩家记录
            Log.Info("保存当前Unit信息到数据库" + unit);

            reply();

            
            
            unit.DomainScene().GetComponent<UnitComponent>().Remove(unit.Id);
            

            await LocationProxyComponent.Instance.Remove(unit.Id);

 

            await ETTask.CompletedTask;
        }
    }
}
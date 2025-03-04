using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class C2M_UnloadItemHandler: AMActorLocationRpcHandler<Unit, C2M_UnloadItem, M2C_UnloadItem>
    {
        protected override async ETTask Run(Unit unit, C2M_UnloadItem request, M2C_UnloadItem response, Action reply)
        {
            //需有该位置的道具  ，看看背包满没满。 卸载  
            BagComponent bagCpt = unit.GetComponent<BagComponent>();
            var roleEqpCpt = unit.GetComponent<RoleEquipComponent>();
            if (bagCpt.IsMaxCapacity())
            {
                Log.Error("背包满了，无法把挤掉装备放入背包，导致穿戴新装备失败！");
                response.Error = ErrorCode.ERR_UnloadItemFail;
                reply();
                return;
            }
            if (!roleEqpCpt.IsContainItem(request.roleEquipPosition))
            {
                Log.Error("托衣失败，在旧的位置没有衣服");
                response.Error = ErrorCode.ERR_UnloadItemFail;
                reply();
                return;
            }
         
            var oldItemEqp = roleEqpCpt.GetItem(request.roleEquipPosition);
            if (!bagCpt.IsCanAddItem(oldItemEqp))
            {
                Log.Error("背包尝试添加脱下来的衣服失败，" + request.roleEquipPosition);
                response.Error = ErrorCode.ERR_DressUpItemFail;
                reply();
                return;
            }

            oldItemEqp = roleEqpCpt.UnloadItem(request.roleEquipPosition);
            bagCpt.AddItem(oldItemEqp);

            reply();
            await ETTask.CompletedTask;
        }
    }
}
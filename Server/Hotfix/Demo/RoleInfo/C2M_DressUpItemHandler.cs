using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class C2M_DressUpItemHandler: AMActorLocationRpcHandler<Unit, C2M_DressUpItem, M2C_DressUpItem>
    {
        protected override async ETTask Run(Unit unit, C2M_DressUpItem request, M2C_DressUpItem response, Action reply)
        {
            BagComponent bagCpt = unit.GetComponent<BagComponent>();
            var item = bagCpt.GetItem(request.ItemBagUid);
            if (item == null)
            {
                Log.Error("穿衣失败。背包里面没有改ItemId" + request.ItemBagUid);
                response.Error = ErrorCode.ERR_DressUpItemFail;
                reply();
                return;
            }

            var roleEqpCpt = unit.GetComponent<RoleEquipComponent>();

            // 把玩家从装备栏上有得道具 卸载掉。，  属性 扣除
            if (roleEqpCpt.IsContainItem(item.Config.EquipPosition))
            {
                var oldItemEqp = roleEqpCpt.GetItem(item.Config.EquipPosition);
                if (!bagCpt.IsCanAddItem(oldItemEqp))
                {
                    Log.Error("背包添加旧衣失败，" + item.Config.EquipPosition);
                    response.Error = ErrorCode.ERR_DressUpItemFail;
                    reply();
                    return;
                }

                oldItemEqp = roleEqpCpt.UnloadItem(item.Config.EquipPosition);
                bagCpt.AddItem(oldItemEqp);
            }

            // 穿戴上 新的道具 。属性 添加 
            if (!roleEqpCpt.DressUpItem(item))
            {
                Log.Error("穿衣失败。背包里面没有改ItemId" + request.ItemBagUid);
                response.Error = ErrorCode.ERR_DressUpItemFail;
                reply();
                return;
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}
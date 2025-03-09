using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class C2M_ReceiveProductionItemHandler: AMActorLocationRpcHandler<Unit, C2M_ReceiveProductionItem, M2C_ReceiveProductionItem>
    {
        protected override async ETTask Run(Unit unit, C2M_ReceiveProductionItem request, M2C_ReceiveProductionItem response, Action reply)
        {
            // 属性 值修改。保存缓存服以及 数据库（定时把缓存服数据存储到数据库一次。这样不用反复调用数据库了）  

            var bagCpt = unit.GetComponent<BagComponent>();
            if (bagCpt.IsMaxCapacity())
            {
                Log.Error($" 背包目前是满的不足以接收新的物品");
                response.Error = ErrorCode.ERR_ReceiveProductionFail;
                reply();
                return;
            }

            var forgenCpt = unit.GetComponent<ForgeComponent>();
            if (!forgenCpt.IsHasProductionFinishById(request.ForgeProductionId))
            {
                Log.Error($"物品还没有制作完成");
                response.Error = ErrorCode.ERR_ReceiveProductionFail;
                reply();
                return;
            }

            var produciton = forgenCpt.GetProductionById(request.ForgeProductionId);

            var item = bagCpt.AddItemByConfigID(produciton.Config.ItemConfigId);

            
            forgenCpt.RemoveProductionByid(request.ForgeProductionId);

            reply();
            await ETTask.CompletedTask;
        }
    }
}
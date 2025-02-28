using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class C2M_SellItemHandler: AMActorLocationRpcHandler<Unit, C2M_SellItem, M2C_SellItem>
    {
        protected override async ETTask Run(Unit unit, C2M_SellItem request, M2C_SellItem response, Action reply)
        {
            // 属性 值修改。保存缓存服以及 数据库（定时把缓存服数据存储到数据库一次。这样不用反复调用数据库了）  

            var bagCpt = unit.GetComponent<BagComponent>();
            var item = bagCpt.GetItem(request.ItemUid);
            if (item == null)
            {
                Log.Error($" 售卖失败!背包不存在这个道具id: {request.ItemUid}");
                response.Error = ErrorCode.ERR_SellItemFail;
                reply();
                return;
            }

            //移除道具
            bagCpt.RemoveItem(item,true);

            //增加金币 
            var numCpt = unit.GetComponent<NumericComponent>();

            numCpt[NumericType.Gold] += item.Config.SellBasePrice;

            reply();
            await ETTask.CompletedTask;
        }
    }
}
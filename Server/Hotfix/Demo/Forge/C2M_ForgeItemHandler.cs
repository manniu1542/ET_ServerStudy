using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    [FriendClassAttribute(typeof (ET.ForgeComponent))]
    public class C2M_ForgeItemHandler: AMActorLocationRpcHandler<Unit, C2M_ForgeItem, M2C_ForgeItem>
    {
        protected override async ETTask Run(Unit unit, C2M_ForgeItem request, M2C_ForgeItem response, Action reply)
        {
            var config = ForgeProductionConfigCategory.Instance.Get(request.ForgeProductionConfigId);
            if (config == null)
            {
                Log.Error($"没有该打造的配置表:{request.ForgeProductionConfigId}");
                response.Error = ErrorCode.ERR_ForgeItemFail;
                reply();
                return;
            }

            var numCpt = unit.GetComponent<NumericComponent>();
            int level = numCpt.GetAsInt(NumericType.Level);
            int canForgeCount = PlayerLevelConfigCategory.Instance.Get(level).ForgeTheNumOfQueues;

            var forgeCpt = unit.GetComponent<ForgeComponent>();

            if (canForgeCount <= forgeCpt.dicProductions.Count)
            {
                Log.Error($" 没有多余的打造位置了");
                response.Error = ErrorCode.ERR_ForgeItemFail;
                reply();
                return;
            }

            long curConsumCount = numCpt[config.ConsumId];
            if (curConsumCount < config.ConsumeCount)
            {
                Log.Error($"所需材料{config.ConsumId}不足,拥有{curConsumCount},所需{config.ConsumeCount}");
                response.Error = ErrorCode.ERR_ForgeItemFail;
                reply();
                return;
            }

            numCpt[config.ConsumId] -= config.ConsumeCount;

            var production = forgeCpt.AddProductionByConfig(request.ForgeProductionConfigId);
            response.ForgeProInfo = production.ToMsgData();

            Game.EventSystem.Publish(new EventType.ForgeProductionGameTask()
            {
                UnitInstanceId = unit.InstanceId, ProductionID = request.ForgeProductionConfigId
            });
            reply();
            await ETTask.CompletedTask;
        }
    }
}
using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class C2M_GoToAdventureHandler: AMActorLocationRpcHandler<Unit, C2M_GoToAdventure, M2C_GoToAdventure>
    {
        protected override async ETTask Run(Unit unit, C2M_GoToAdventure request, M2C_GoToAdventure response, Action reply)
        {
            //是否有该管卡，
            var config = BattleLevelConfigCategory.Instance.Get(request.BattleLevelConfigID);
            if (config == null)
            {
                Log.Error("不可以进入战斗，没有该管卡在表里：" + request.BattleLevelConfigID);
                response.Error = ErrorCode.ERR_CantGoToAdventure;
                reply();
                return;
            }

            //查看当前可加点数的值 是否 可用。
            var numCpt = unit.GetComponent<NumericComponent>();
            var adventureState = numCpt.GetAsInt(NumericType.AdventureState);
            if (adventureState != 0)
            {
                Log.Error("不可以进入战斗，当前玩家战斗状态不是闲置状态：" + adventureState);
                response.Error = ErrorCode.ERR_CantGoToAdventure;
                reply();
                return;
            }

            //当前的等级是否可以进入该关卡
            var level = numCpt.GetAsInt(NumericType.Level);
            bool isOpenAdventureLevel = level >= config.MiniEnterLevel[0] && level < config.MiniEnterLevel[1];
            if (!isOpenAdventureLevel)
            {
                Log.Error(
                    "不可以进入战斗，当前玩家不满足进入该关卡：玩家等级是" + isOpenAdventureLevel + "管卡限制等级是：" + config.MiniEnterLevel[0] + "-" + config.MiniEnterLevel[1]);
                response.Error = ErrorCode.ERR_CantGoToAdventure;
                reply();
                return;
            }

            //开始战斗
            numCpt.Set(NumericType.AdventureStartTime, TimeHelper.ServerNow());
            numCpt.Set(NumericType.AdventureState, request.BattleLevelConfigID);
            reply();

            await ETTask.CompletedTask;

        }
    }
}
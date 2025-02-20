using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class C2M_AdventureEndHandler: AMActorLocationRpcHandler<Unit, C2M_AdventureEnd, M2C_AdventureEnd>
    {
        protected override async ETTask Run(Unit unit, C2M_AdventureEnd request, M2C_AdventureEnd response, Action reply)
        {
            //验证 关卡是否 有  ，验
            var numCpt = unit.GetComponent<NumericComponent>();
            var levelId = numCpt.GetAsInt(NumericType.AdventureState);
            var config = BattleLevelConfigCategory.Instance.Get(levelId);
            if (config == null)
            {
                Log.Error("不可以进入战斗，没有该管卡在表里：" + levelId);
                response.Error = ErrorCode.ERR_AdventureEndCheckCant;
                reply();
                return;
            }

            // 战斗回合数 
            if (request.RoundCount <= 0)
            {
                Log.Error("战斗回合数有问题" + request.RoundCount);
                response.Error = ErrorCode.ERR_AdventureEndCheckCant;
                reply();
                return;
            }

            //战斗结果有问题
            if (request.AdventureBattleRoundState <= 0 || request.AdventureBattleRoundState > 2)
            {
                Log.Error("战斗结果有问题" + request.AdventureBattleRoundState);
                response.Error = ErrorCode.ERR_AdventureEndCheckCant;
                reply();
                return;
            }

            var adcCpt = unit.GetComponent<AdventureCheckComponent>();
            if (request.AdventureBattleRoundState == 1 && !adcCpt.CheckWinBattle(request.RoundCount, levelId))
            {
                Log.Error("战斗验证没有通过！");
                response.Error = ErrorCode.ERR_AdventureEndCheckCant;
                reply();
                return;
            }

            //战斗结束！
            numCpt.Set(NumericType.AdventureState, 0);
            numCpt.Set(NumericType.AdventureStartTime, 0);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
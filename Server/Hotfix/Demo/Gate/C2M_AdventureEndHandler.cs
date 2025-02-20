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

            //查看当前可加点数的值 是否 可用。
            var numCpt = unit.GetComponent<NumericComponent>();
            if (numCpt == null)
            {
                Log.Error("不可以进入战斗，没有该管卡在表里：");
                response.Error = ErrorCode.ERR_CantGoToAdventure;
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
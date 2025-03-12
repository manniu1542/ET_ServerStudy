using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    [FriendClassAttribute(typeof (ET.ForgeComponent))]
    public class C2M_ReceiveGameTaskRewardHandler: AMActorLocationRpcHandler<Unit, C2M_ReceiveGameTaskReward, M2C_ReceiveGameTaskReward>
    {
        protected override async ETTask Run(Unit unit, C2M_ReceiveGameTaskReward request, M2C_ReceiveGameTaskReward response, Action reply)
        {
            var taskCpt = unit.GetComponent<TaskComponent>();

            if (!taskCpt.TryReceiveTaskReward(request.TaskConfigId))
            {
                response.Error = ErrorCode.ERR_ReceiveGameTaskRewardFail;
                reply();
                return;
            }
            
            taskCpt.ReceiveGameTaskHandler(request.TaskConfigId);
           
            //获取奖励
            var config = TaskConfigCategory.Instance.Get(request.TaskConfigId);
            var numCpt = unit.GetComponent<NumericComponent>();
            numCpt[NumericType.Gold] += config.RewardGoldCount;

            reply();
            await ETTask.CompletedTask;
        }
    }
}
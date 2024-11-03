using System;

namespace ET
{
    [ActorMessageHandler]
    public class ForcePlayerLogOutHandler : AMActorRpcHandler<Scene, A2L_ForcePlayerLogOut, L2A_ForcePlayerLogOut>
    {
        protected override async ETTask Run(Scene scene, A2L_ForcePlayerLogOut request, L2A_ForcePlayerLogOut response, Action reply)
        {
            long acountId = request.AccountID;
            //防止多个 客户端，同一时刻 请求
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterPlayerLogOut, acountId.GetHashCode()))
            {
                LoginAccountInZoneRecordComponent loginAccountZone = scene.GetComponent<LoginAccountInZoneRecordComponent>();


                //该玩家不在中心服务器注册，证明他现在没有别的账号登陆了中心服务器
                if (!loginAccountZone.IsExit(acountId))
                {
                    reply();
                    return;
                }
                int zone = loginAccountZone.Get(acountId);
                //获取该账号使用了那个网关
                StartSceneConfig config = RealmGateAddressHelper.GetGate(acountId, zone);


                G2L_ForcePlayerDisconnect forcePlayerLogOut = (G2L_ForcePlayerDisconnect)await MessageHelper.CallActor(
                     config.InstanceId, new L2G_ForcePlayerDisconnect() { AccountID = request.AccountID });


                response.Error = forcePlayerLogOut.Error;
                reply();



            }
        }
    }
}

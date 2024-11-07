using System;

namespace ET
{
    [ActorMessageHandler]
    public class ForcePlayerDisconnectHandler : AMActorRpcHandler<Scene, L2G_ForcePlayerDisconnect, G2L_ForcePlayerDisconnect>
    {
        protected override async ETTask Run(Scene scene, L2G_ForcePlayerDisconnect request, G2L_ForcePlayerDisconnect response, Action reply)
        {
        
            long acountId = request.AccountID;
            //防止多个 客户端，同一时刻 请求
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterPlayerLogOut, acountId.GetHashCode()))
            {

                PlayerComponent playerCpt = scene.GetComponent<PlayerComponent>();

                Player player = playerCpt.Get(request.AccountID);


                 
                if (player == null)
                {
                    reply();
                    return;
                }

                playerCpt.Remove(acountId);

                player.Dispose();


                //通知登录的到网关的玩家下线。发送协议 ，请求在map服务器的账号是否有该玩家  找到 Map服务器得到Unit 推送下线通知。

                //MessageHelper.SendToClient(player.GetComponent<Unit>(), new G2C_ForcePlayerDisconnect());


            }


        }
    }
}

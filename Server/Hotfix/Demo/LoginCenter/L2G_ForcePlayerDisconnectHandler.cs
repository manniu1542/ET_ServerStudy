using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    [ActorMessageHandler]
    public class L2G_ForcePlayerDisconnectHandler: AMActorRpcHandler<Scene, L2G_ForcePlayerDisconnect, G2L_ForcePlayerDisconnect>
    {
        protected override async ETTask Run(Scene scene, L2G_ForcePlayerDisconnect request, G2L_ForcePlayerDisconnect response, Action reply)
        {
            //请求的服务器类型
            SceneType st = scene.SceneType;
            if (st != SceneType.Gate)
            {
                response.Error = ErrorCode.ERR_LoginSceneSever;
                reply();

                Log.Error("请求的场景服务器错误！" + st);
                return;
            }

            long acountId = request.AccountID;

  
            //防止多个 客户端，同一时刻 请求
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, acountId.GetHashCode()))
            {
                PlayerComponent playerCpt = scene.GetComponent<PlayerComponent>();
                //TODO：令牌怎么处理
                scene.DomainScene().GetComponent<GateSessionKeyComponent>().Remove(request.AccountID);

                Player player = playerCpt.Get(request.AccountID);

                if (player == null)
                {
                    reply();
                    return;
                }
                
                //服务端定时移除数据(定时可以保证 如果玩家 在短时间内连接回来 ，服务器依然有玩家的数据。从而实现断线重连功能)
                player.AddComponent<PlayerLineOffComponent>();

                
                Session sessionPalyer = Game.EventSystem.Get(player.SessionInstanceId) as Session;
                if (sessionPalyer != null && !sessionPalyer.IsDisposed)
                {            //告知客户端下线的 消息
                    sessionPalyer?.Send(new G2C_ForcePlayerDisconnect());
                    //移除session自己
                    sessionPalyer?.Disconnect();
                    SessionPlayerComponent spc = sessionPalyer.GetComponent<SessionPlayerComponent>();
                    if (spc != null)
                    {
                        spc.IsLoginAgain = true;
                    }
                    
                }
    

                reply();
            }
        }
    }
}
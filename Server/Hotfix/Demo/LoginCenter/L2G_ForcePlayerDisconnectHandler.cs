using System;

namespace ET
{
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

            PlayerComponent playerCpt = scene.GetComponent<PlayerComponent>();
            //TODO：令牌怎么处理
            scene.DomainScene().GetComponent<GateSessionKeyComponent>().Remove(request.AccountID);

            Player player = playerCpt.Get(request.AccountID);

            if (player == null)
            {
                reply();
                return;
            }

            long id = player.InstanceId;
            //防止多个 客户端，同一时刻 请求
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, acountId.GetHashCode()))
            {
                //调用很多次，进来的账号已经不是同一个 直接return出去
                if (id == 0 || id != player.InstanceId)
                {
                    reply();
                    return;
                }

                switch (player.State)
                {
                    case PlayerState.Disconect:
                        Log.Error("玩家已经下线了，无需再次下线！" + player);
                        reply();
                        return;
                    case PlayerState.Gate:
                        //获取到网关的session让玩家下线

                        Session sessionPalyer = Game.EventSystem.Get(player.SessionInstanceId) as Session;

                        //告知客户端下线的 消息
                        sessionPalyer.Send(new G2C_ForcePlayerDisconnect());

                        //服务端定时移除数据(定时可以保证 如果玩家 在短时间内连接回来 ，服务器依然有玩家的数据。从而实现断线重连功能h)
                        player.AddComponent<PlayerLineOffComponent>();

                        break;
                    case PlayerState.Game:
                        //在游戏中  要把map中角色也提出下线
                        // 请求在map服务器的账号是否有该玩家 找到 Map服务器得到Unit 推送下线通知。

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
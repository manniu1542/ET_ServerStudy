using System;

namespace ET
{
    [FriendClass(typeof(SessionPlayerComponent))]
    [FriendClass(typeof(RoleInfo))]
    public class LinkGateLoginHandler : AMRpcHandler<C2G_LinkGateLogin, G2C_LinkGateLogin>
    {
        protected override async ETTask Run(Session session, C2G_LinkGateLogin request, G2C_LinkGateLogin response, Action reply)
        {

            //移除超时 还未响应的 组件 (只在这里移除，LoginAccount是Account服务器的第一个消息，需要移除session超时)
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            //请求的服务器类型
            SceneType st = session.DomainScene().SceneType;
            if (st != SceneType.Gate)
            {
                response.Error = ErrorCode.ERR_LoginSceneSever;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("请求的账号，场景服务器错误！" + st);
                return;
            }

            Scene scene = session.DomainScene();
            //对比key
            string key = scene.GetComponent<GateSessionKeyComponent>().Get(request.AccountId);

            if (string.IsNullOrEmpty(key) || key != request.SessionKey)
            {
                response.Error = ErrorCode.ERR_GateKeyError;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("该账号在网关服务器上已经下线！");
                return;
            }
            session.DomainScene().GetComponent<GateSessionKeyComponent>().Remove(request.AccountId);



            //不是多余,using执行完就dispose了。   （多余每次登录的 session 都不同。所以 添加这个 毫无用处。只有在相同的session下才 有效）
            var rsc = session.GetComponent<RepeatClickServerComponent>();
            if (rsc != null)
            {
                response.Error = ErrorCode.ERR_LoginRepaetReq;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("重复请求！客户端防不住的通过处理");
                return;

            }


            long sessionid = session.InstanceId;
            //防止一个客户端的 等待时候的重复点击。
            using (session.AddComponent<RepeatClickServerComponent>())
            {
                int ahc = request.AccountId.GetHashCode();

                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, ahc))
                {
                    if (sessionid != session.InstanceId)
                    {
                        response.Error = ErrorCode.ERR_OtherMechineLoginGateReq;
                        reply();
                        session.Disconnect().Coroutine();
                        Log.Error("其他机器也进入登录网关请求");
                        return;
                    }

                    StartSceneConfig realmConfig = StartSceneConfigCategory.Instance.LoginCenters[session.DomainZone()];

                    L2G_RigestLoginCenterPlayer rigestLoginCenter = await MessageHelper.CallActor(realmConfig.InstanceId, new G2L_RigestLoginCenterPlayer() { AccountID = request.AccountId })
                        as L2G_RigestLoginCenterPlayer;


                    if (rigestLoginCenter.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = rigestLoginCenter.Error;
                        reply();
                        session.Disconnect().Coroutine();
                        return;
                    }
                    PlayerComponent scrPlayerCpt = scene.GetComponent<PlayerComponent>();
                    Player player = scrPlayerCpt.Get(request.AccountId);

                    if (player == null)
                    {
                        player = scrPlayerCpt.AddChildWithId<Player, long, long>(request.RoleId, request.AccountId, request.RoleId);
                        player.State = PlayerState.Gate;
                        scrPlayerCpt.Add(player);
                        //通信组件
                        session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
                    }
                    else
                    {
                        //移除的  游戏玩家下线组件
                        player.RemoveComponent<PlayerLineOffComponent>();
                    }

                    //给玩家与 通信Session绑定起来

                    //玩家可以得到Session
                    player.SessionInstanceId = session.InstanceId;

                    SessionPlayerComponent spc = session.AddComponent<SessionPlayerComponent>();
                    //获取到session，可以获取到玩家组件
                    spc.PlayerInstaceId = player.InstanceId;
                    //知道玩家是哪一个，玩家的id就是RoleId
                    spc.AccountID = player.Account;
                    spc.PlayerID = player.Id;
                }
            }




            reply();

            await ETTask.CompletedTask;
        }
    }
}

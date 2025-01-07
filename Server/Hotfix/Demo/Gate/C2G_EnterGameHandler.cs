using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class C2G_EnterGameHandler: AMRpcHandler<C2G_EnterGame, G2C_EnterGame>
    {
        protected override async ETTask Run(Session session, C2G_EnterGame request, G2C_EnterGame response, Action reply)
        {
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

            //获取服务器上 在 登录 网关时候已经挂载的 玩家session与player绑定脚本获取失败！
            SessionPlayerComponent spc = session.GetComponent<SessionPlayerComponent>();
            if (null == spc)
            {
                response.Error = ErrorCode.ERR_SessionPlayerError;
                reply();
                return;
            }

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

            var player = Game.EventSystem.Get(spc.PlayerInstaceId) as Player;
            long playerInstanceId = player.InstanceId;
            //防止一个客户端的 等待时候的重复点击。
            using (session.AddComponent<RepeatClickServerComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.Account.GetHashCode()))
                {
                    if (player.IsDisposed || playerInstanceId == 0)
                    {
                        response.Error = ErrorCode.ERR_PlayerDisposeError;
                        session.Disconnect().Coroutine();
                        Log.Error("该玩家的数据已经被释放掉不可以再次登录");
                        return;
                    }

                    var SessionState = session.GetComponent<SessionStateComponent>();
                    if (SessionState != null)
                    {
                        if (SessionState.curState == ET.SessionState.Game)
                        {
                            response.Error = ErrorCode.ERR_SessionEnterGameRealdy;
                            reply();
                            session.Disconnect().Coroutine();
                            Log.Error("该用户的Seesion已经登录游戏了");
                            return;
                        }
                    }
                    else
                    {
                        SessionState = session.AddComponent<SessionStateComponent>();
                    }

                    //1.创建 GateMap场景 来创建unit初始 场景，然后看看这个GateMap是每次登录 都创建这个场景么？，
                    //2.传送 玩家到 Map1场景，再把该账号的UnitId赋值，以及 player，session的状态修改了
                    //有人 顶号登录， 该账号再游戏内 。需要先把 老人 踢出游戏， （异常处理， 顶号 逻辑有问题。 需要清理自己的 登录请求）
                    if (player.State == PlayerState.Game)
                    {
                        try
                        {
                            M2G_RequestEnterGameState reqEnter =
                                    await MessageHelper.CallLocationActor(player.UintId,
                                        new G2M_RequestEnterGameState()) as M2G_RequestEnterGameState;

                            if (reqEnter.Error == ErrorCode.ERR_Success)
                            {
                                Log.Error("二次登录成功！，要把当前用户进入的丢进map场景，老的session给移除下线！");
                                reply();
                                return;
                            }

                            //获取到该玩家的session
                            Session sessionOther = Game.EventSystem.Get(player.SessionInstanceId) as Session;
                            sessionOther?.Send(new G2C_ForcePlayerDisconnect());
                            await SessionDisconnectHelp.LetPlayLeave(player, true);
                            sessionOther.Disconnect().Coroutine();
                            reply();
                        }
                        catch (Exception e)
                        {
                            response.Error = ErrorCode.ERR_SessionEnterGameRealdy;
                            reply();
                            await SessionDisconnectHelp.LetPlayLeave(player, true);
                            //给这个玩家踢出游戏中 
                            session.Disconnect().Coroutine();
                            Log.Error("二次登录失败！！" + e);
                            throw;
                        }

                        return;
                    }

                    try
                    {
                        GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
                        //创建一个动态创景
                        gateMapComponent.Scene = await SceneFactory.Create(gateMapComponent, "GateMap", SceneType.Map);

                        Unit unit = UnitFactory.Create(gateMapComponent.Scene, player.Id, UnitType.Player);
                        //使定位服务器知道 他是在哪个Gate网关上连接着的
                        unit.AddComponent<UnitGateComponent, long>(session.InstanceId);
                        //TODO:当前只有一个服
                        int zone = 1;
                        StartSceneConfig realmConfig = StartSceneConfigCategory.Instance.GetBySceneName(zone, "Map1");
                        await TransferHelper.Transfer(unit, realmConfig.InstanceId, realmConfig.Name);

                        player.UintId = unit.Id;

                        player.State = PlayerState.Game;

                        SessionState.curState = ET.SessionState.Game;

                        response.UnitID = unit.Id;
                        reply();
                    }
                    catch (Exception e)
                    {
                        response.Error = ErrorCode.ERR_EnterGameError;
                        reply();
                        //给这个玩家踢出游戏中 
                        session.Disconnect().Coroutine();

                        Log.Error($"登录异常 玩家账号：{player.Account},玩家的用户id{player.Id},错误原因：{e}");
                    }
                }
            }

            await ETTask.CompletedTask;
        }
    }
}
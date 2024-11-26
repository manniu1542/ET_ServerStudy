using System;

namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public class GetGateHandler : AMRpcHandler<C2R_GetGate, R2C_GetGate>
    {
        protected override async ETTask Run(Session session, C2R_GetGate request, R2C_GetGate response, Action reply)
        {


            //让负载均衡服务器 通过 SessionAcceptTimeoutComponent  自动移除 ，
            //session.RemoveComponent<SessionAcceptTimeoutComponent>();


            //请求的服务器类型
            SceneType st = session.DomainScene().SceneType;
            if (st != SceneType.Realm)
            {
                response.Error = ErrorCode.ERR_LoginSceneSever;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("请求的账号，场景服务器错误！" + st);
                return;
            }


            //对比token
            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);

            if (string.IsNullOrEmpty(token) || token != request.Token)
            {
                response.Error = ErrorCode.ERR_RealmKeyError;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("该账号在负载均衡服务器上已经下线！");
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

            //防止一个客户端的 等待时候的重复点击。
            using (session.AddComponent<RepeatClickServerComponent>())
            {
                //为什么用跟登录同一个协程锁呢。避免登录的时候，有玩家在过取realm网关服务器。（此时，刚登陆的号会把这个正在获取realm网关的人踢下线）
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCommonAccount, request.AccountId))
                {
                   
                    StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(request.AccountId, session.DomainZone());

                    G2R_GetGatekey Gatekey = await MessageHelper.CallActor(gateConfig.InstanceId, new R2G_GetGatekey() { AccountID = request.AccountId })
                        as G2R_GetGatekey;


                    if (Gatekey.Error != ErrorCode.ERR_Success)
                    {
                        Log.Error("请求获取负载均衡服务器的Key报错：" + Gatekey.Error);
                        response.Error = Gatekey.Error;
                        reply();
                        return;
                    }

                    response.AdressGate = gateConfig.OuterIPPort.ToString();
                    response.KeyGate = Gatekey.KeyRealmGate;



                    reply();



                }


            }


            await ETTask.CompletedTask;
        }
    }
}

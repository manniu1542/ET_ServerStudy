using System;

namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public class DeleRoleInfoHandler : AMRpcHandler<C2A_DeleRoleInfo, A2C_DeleRoleInfo>
    {
        protected override async ETTask Run(Session session, C2A_DeleRoleInfo request, A2C_DeleRoleInfo response, Action reply)
        {


            //请求的服务器类型
            SceneType st = session.DomainScene().SceneType;
            if (st != SceneType.Account)
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
                response.Error = ErrorCode.ERR_TokenError;
                reply();
                session.Disconnect().Coroutine();
                Log.Error("该账号服务器上已经下线！");
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

                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.AccountCreateRoleInfo, request.AccountId))
                {


                    var dbc = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());

                    var list = await dbc.Query<RoleInfo>(d => d.AccountId == request.AccountId && d.ServerId == request.ServerId);
                    if (list == null || list.Count == 0)
                    {

                        response.Error = ErrorCode.ERR_RoleInfoNotExistDB;
                        reply();
                        Log.Error("角色不存在数据库！");
                        return;
                    }

                    RoleInfo roleInfo = list[0];
           
                    roleInfo.State = RoleInfoState.Freeze;
                    await dbc.Save(roleInfo);
                    reply();

                    roleInfo?.Dispose();

                }


            }


            await ETTask.CompletedTask;
        }
    }
}

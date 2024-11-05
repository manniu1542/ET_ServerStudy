using System;

namespace ET
{
    [FriendClass(typeof(ServerInfoManager))]
    public class GetServerInfoHandler : AMRpcHandler<C2A_GetServerInfo, A2C_GetServerInfo>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfo request, A2C_GetServerInfo response, Action reply)
        {

            //移除超时 还未响应的 组件
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

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







            foreach (var item in session.DomainScene().GetComponent<ServerInfoManager>().ServerInfoList)
            {
                response.ListServerInfo.Add(item.ToMessage());
            }


            reply();




            await ETTask.CompletedTask;
        }
    }
}

using System;

namespace ET
{
    [ActorMessageHandler]
    public class GetRealmInfoHandler : AMActorRpcHandler<Scene, A2R_GetRealmInfo, R2A_GetRealmInfo>
    {
        protected override async ETTask Run(Scene scene, A2R_GetRealmInfo request, R2A_GetRealmInfo response, Action reply)
        {

   
            //请求的服务器类型
            SceneType st = scene.SceneType;
            if (st != SceneType.Realm)
            {
                response.Error = ErrorCode.ERR_LoginSceneSever;
                reply();

                Log.Error("请求的场景服务器错误！" + st);
                return;
            }
            long acountId = request.AccountID;
            string key = TimeHelper.ServerNow().ToString() + RandomHelper.RandInt64().ToString();

   
            //添加负载均衡服务器的账号 密钥
            var scrRealmToken = scene.GetComponent<TokenComponent>();
            scrRealmToken.Remove(acountId);
            scrRealmToken.Add(acountId, key);



            response.KeyRealmGate = key;
            reply();

            await ETTask.CompletedTask;
        }
    }
}

using System;

namespace ET
{
    [ActorMessageHandler]
    public class R2G_RealmToGetGateKeyHandler : AMActorRpcHandler<Scene, R2G_GetGatekey, G2R_GetGatekey>
    {
        protected override async ETTask Run(Scene scene, R2G_GetGatekey request, G2R_GetGatekey response, Action reply)
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
            string key = RandomHelper.RandInt64().ToString() + TimeHelper.ServerNow().ToString();


            //添加负载均衡服务器的账号 密钥
            var scrRealmToken = scene.GetComponent<GateSessionKeyComponent>();
            scrRealmToken.Remove(acountId);
            scrRealmToken.Add(acountId, key);



            response.KeyRealmGate = key;
            reply();

            await ETTask.CompletedTask;
        }
    }
}

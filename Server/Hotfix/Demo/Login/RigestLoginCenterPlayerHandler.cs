using System;

namespace ET
{
    [ActorMessageHandler]
    public class RigestLoginCenterPlayerHandler : AMActorRpcHandler<Scene, G2L_RigestLoginCenterPlayer, L2G_RigestLoginCenterPlayer>
    {
        protected override async ETTask Run(Scene scene, G2L_RigestLoginCenterPlayer request, L2G_RigestLoginCenterPlayer response, Action reply)
        {

   
            //请求的服务器类型
            SceneType st = scene.SceneType;
            if (st != SceneType.LoginCenter)
            {
                response.Error = ErrorCode.ERR_LoginSceneSever;
                reply();

                Log.Error("请求的场景服务器错误！" + st);
                return;
            }
        

            //添加 账号 服务器id
            var scrLoginAccountInZoneRecord = scene.GetComponent<LoginAccountInDistrictRecordComponent>();
            scrLoginAccountInZoneRecord.Remove(request.AccountID);
            scrLoginAccountInZoneRecord.Add(request.AccountID, request.ServerId);







     
            reply();

            await ETTask.CompletedTask;
        }
    }
}

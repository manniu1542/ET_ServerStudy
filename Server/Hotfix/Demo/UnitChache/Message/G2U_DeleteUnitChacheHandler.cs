using System;

namespace ET
{
    [ActorMessageHandler]
    public class G2U_DeleteUnitChacheHandler: AMActorRpcHandler<Scene, G2U_DeleteUnitChache, U2G_DeleteUnitChache>
    {
        protected override async ETTask Run(Scene scene, G2U_DeleteUnitChache request, U2G_DeleteUnitChache response, Action reply)
        {
            //请求的服务器类型
            SceneType st = scene.SceneType;
            if (st != SceneType.UnitChache)
            {
                response.Error = ErrorCode.ERR_SwitchSceneSever;
                reply();

                Log.Error("请求的场景服务器错误！" + st);
                return;
            }
            AddOrUpdateUnitChache(scene,request,response).Coroutine();
          

            reply();
        }
        
        protected async   ETTask  AddOrUpdateUnitChache(Scene scene, G2U_DeleteUnitChache request, U2G_DeleteUnitChache response)
        {
            UnitChacheComponent ucc = scene.GetComponent<UnitChacheComponent>();
         
            
        }
    }
}
using System;

namespace ET
{
    [ActorMessageHandler]
    public class G2U_GetUnitChacheHandler: AMActorRpcHandler<Scene, G2U_GetUnitChache, U2G_GetUnitChache>
    {
        protected override async ETTask Run(Scene scene, G2U_GetUnitChache request, U2G_GetUnitChache response, Action reply)
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

            
            
            
            
            reply();
        }
        
    
    }
}
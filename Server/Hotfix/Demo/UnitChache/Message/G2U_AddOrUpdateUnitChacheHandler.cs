using System;

namespace ET
{
    [ActorMessageHandler]
    public class G2U_AddOrUpdateUnitChacheHandler: AMActorRpcHandler<Scene, G2U_AddOrUpdateUnitChache, U2G_AddOrUpdateUnitChache>
    {
        protected override async ETTask Run(Scene scene, G2U_AddOrUpdateUnitChache request, U2G_AddOrUpdateUnitChache response, Action reply)
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
        
        protected async   ETTask  AddOrUpdateUnitChache(Scene scene, G2U_AddOrUpdateUnitChache request, U2G_AddOrUpdateUnitChache response)
        {
            UnitChacheComponent ucc = scene.GetComponent<UnitChacheComponent>();
            //是在玩家下线了 再更新还是每次做修改都操作数据库呢？
            //从 unitComponent 中拿出 请求的     request.UnitId 所要的 组件 实体，更新一下，  如果拿不出来，就需要 从数据库里面取出来并更新。

            using (ListComponent<Entity> list = ListComponent<Entity>.Create())
            {
                //还原 unit身长继承的组件
                for (int i = 0; i < request.EntityType.Count; i++)
                {
                    Type type = Game.EventSystem.GetType(request.EntityType[i]);
                    Entity entity = MongoHelper.FromBson(type, request.EntityBytes[i]) as Entity;
                    list.Add(entity);
                }

                await ucc.AddOrUpdate(request.UnitId, list);
            }
            
            
        }
    }
}